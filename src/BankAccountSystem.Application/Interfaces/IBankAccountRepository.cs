using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Application.Interfaces;

public interface IBankAccountRepository
{
    BankAccount? GetById(Guid id);

    IEnumerable<BankAccount> GetAll();

    void Add(BankAccount account);

    void Update(BankAccount account);

    void Delete(Guid id);

    bool Exists(Guid id);
}