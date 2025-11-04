using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Infrastructure.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly Dictionary<Guid, BankAccount> _accounts = [];

    public BankAccount? GetById(Guid id) => _accounts.GetValueOrDefault(id);

    public IEnumerable<BankAccount> GetAll() => _accounts.Values;

    public void Add(BankAccount account)
    {
        if (!_accounts.TryAdd(account.Id, account))
            throw new InvalidOperationException();
    }

    public void Update(BankAccount account)
    {
        if (!_accounts.ContainsKey(account.Id))
            throw new InvalidOperationException();
        _accounts[account.Id] = account;
    }

    public void Delete(Guid id) => _accounts.Remove(id);

    public bool Exists(Guid id) => _accounts.ContainsKey(id);
}