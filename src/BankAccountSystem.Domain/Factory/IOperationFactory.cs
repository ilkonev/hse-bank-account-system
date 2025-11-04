using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Domain.Factory;

public interface IOperationFactory
{
    Operation CreateIncomeOperation(Guid accountId, Money amount, Guid categoryId, string? description = null);

    Operation CreateExpenseOperation(Guid accountId, Money amount, Guid categoryId, string? description = null);
}