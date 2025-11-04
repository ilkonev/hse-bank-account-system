using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.Enums;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Domain.Factory;

public class OperationFactory : IOperationFactory
{
    public Operation CreateIncomeOperation(Guid accountId, Money amount, Guid categoryId, string? description = null)
    {
        return new Operation(OperationType.Income, accountId, amount, categoryId, description);
    }

    public Operation CreateExpenseOperation(Guid accountId, Money amount, Guid categoryId, string? description = null)
    {
        return new Operation(OperationType.Expense, accountId, amount, categoryId, description);
    }
}