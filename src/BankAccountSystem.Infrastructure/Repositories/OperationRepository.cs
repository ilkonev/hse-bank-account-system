using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Infrastructure.Repositories;

public class OperationRepository : IOperationRepository
{
    private readonly Dictionary<Guid, Operation> _operations = new();

    public Operation? GetById(Guid id) => _operations.GetValueOrDefault(id);

    public IEnumerable<Operation> GetAll() => _operations.Values;

    public void Add(Operation operation)
    {
        if (!_operations.TryAdd(operation.Id, operation))
            throw new InvalidOperationException();
    }

    public IEnumerable<Operation> GetByAccountId(Guid accountId) =>
        _operations.Values.Where(o => o.BankAccountId == accountId);

    public IEnumerable<Operation> GetByDateRange(DateTime start, DateTime end) =>
        _operations.Values.Where(o => o.Date >= start && o.Date <= end);

    public bool Exists(Guid id) => _operations.ContainsKey(id);
}