using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Infrastructure.Repositories;

public class CachedBankAccountRepository : IBankAccountRepository
{
    private readonly IBankAccountRepository _realRepository;
    private readonly Dictionary<Guid, BankAccount> _cache = [];

    private bool _isAllLoaded;

    private List<BankAccount> _allCache = [];

    /// <summary>
    /// .ctor
    /// </summary>
    public CachedBankAccountRepository(IBankAccountRepository realRepository)
    {
        _realRepository = realRepository;
        _isAllLoaded = false;
    }

    public BankAccount? GetById(Guid id)
    {
        Console.WriteLine($"Поиск счета {id} в кэше...");

        if (_cache.TryGetValue(id, out var cachedAccount))
        {
            Console.WriteLine($"Счет {id} найден в кэше");
            return cachedAccount;
        }

        Console.WriteLine($"Счет {id} не найден в кэше, запрос к реальному репозиторию");
        var account = _realRepository.GetById(id);

        if (account == null) return account;

        _cache[id] = account;
        Console.WriteLine($"Счет {id} закэширован");

        return account;
    }

    public IEnumerable<BankAccount> GetAll()
    {
        if (_isAllLoaded)
        {
            Console.WriteLine("Возвращаем все счета из кэша");
            return _allCache;
        }

        Console.WriteLine("Загрузка всех счетов из реального репозитория...");
        var allAccounts = _realRepository.GetAll().ToList();

        _allCache = allAccounts;
        _isAllLoaded = true;

        foreach (var account in allAccounts)
        {
            _cache[account.Id] = account;
        }

        Console.WriteLine($"Все счета ({allAccounts.Count}) закэшированы");
        return _allCache;
    }

    public void Add(BankAccount account)
    {
        Console.WriteLine($"Добавление счета {account.Id} в кэш и репозиторий");
        _realRepository.Add(account);
        _cache[account.Id] = account;
        _isAllLoaded = false;
    }

    public void Update(BankAccount account)
    {
        Console.WriteLine($"Обновление счета {account.Id} в кэше и репозитории");
        _realRepository.Update(account);
        _cache[account.Id] = account;
        _isAllLoaded = false;
    }

    public void Delete(Guid id)
    {
        Console.WriteLine($"Удаление счета {id} из кэша и репозитория");
        _realRepository.Delete(id);
        _cache.Remove(id);
        _isAllLoaded = false;
    }

    public bool Exists(Guid id)
    {
        if (_cache.ContainsKey(id))
        {
            Console.WriteLine($"Счет {id} существует (из кэша)");
            return true;
        }

        var exists = _realRepository.Exists(id);
        Console.WriteLine($"{(exists ? "Да" : "Нет")} Счет {id} {(exists ? "существует" : "не существует")} (из репозитория)");
        return exists;
    }
}