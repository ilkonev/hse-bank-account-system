using BankAccountSystem.Domain.Enums;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Domain.Entities;

public class Operation
{
    public Guid Id { get; private set; }

    public OperationType Type { get; private set; }

    public Guid BankAccountId { get; private set; }

    public Money Amount { get; private set; }

    public DateTime Date { get; private set; }

    public string? Description { get; private set; }

    public Guid CategoryId { get; private set; }


    /// <summary>
    /// .ctor
    /// </summary>
    public Operation(OperationType type, Guid bankAccountId, Money amount, Guid categoryId, string? description = null)
    {
        Id = Guid.NewGuid();
        Type = type;
        BankAccountId = bankAccountId;
        Amount = amount;
        CategoryId = categoryId;
        Date = DateTime.UtcNow;
        Description = description;
    }
}
