using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Application.Interfaces;

public interface IOperationRepository
{
    Operation? GetById(Guid id);

    IEnumerable<Operation> GetAll();

    void Add(Operation operation);

    IEnumerable<Operation> GetByAccountId(Guid accountId);

    IEnumerable<Operation> GetByDateRange(DateTime start, DateTime end);

    bool Exists(Guid id);
}