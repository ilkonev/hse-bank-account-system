using BankAccountSystem.Domain.Enums;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; protected init; }

    public string Name { get; protected set; }

    public Money Balance { get; protected set; }

    private readonly List<Operation> _operations = [];

    public IReadOnlyList<Operation> Operations => _operations.AsReadOnly();

    /// <summary>
    /// .ctor
    /// </summary>
    public BankAccount(string name, Money initialBalance)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Balance = initialBalance;
    }

    public virtual Operation AddIncome(Money amount, Category category, string? description = null)
    {
        if (category.Type != CategoryType.Income)
            throw new InvalidOperationException();

        var operation = new Operation(OperationType.Income, Id, amount, category.Id, description);
        _operations.Add(operation);
        Balance += amount;

        return operation;
    }

    public virtual Operation AddExpense(Money amount, Category category, string? description = null)
    {
        if (category.Type != CategoryType.Expense)
            throw new InvalidOperationException();

        if (Balance.Amount < amount.Amount)
            throw new InvalidOperationException();

        var operation = new Operation(OperationType.Expense, Id, amount, category.Id, description);
        _operations.Add(operation);
        Balance -= amount;

        return operation;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException();

        Name = newName.Trim();
    }

    public void RecalculateBalance()
    {
        var newBalance = new Money(0);
        foreach (var operation in _operations)
        {
            newBalance = operation.Type == OperationType.Income
                ? newBalance + operation.Amount
                : newBalance - operation.Amount;
        }
        Balance = newBalance;
    }

    public decimal GetTotalIncome() =>
        _operations.Where(o => o.Type == OperationType.Income).Sum(o => o.Amount.Amount);

    public decimal GetTotalExpenses() =>
        _operations.Where(o => o.Type == OperationType.Expense).Sum(o => o.Amount.Amount);
}