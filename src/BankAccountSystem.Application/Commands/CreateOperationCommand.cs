using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.Enums;
using BankAccountSystem.Domain.Factory;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Application.Commands;

public class CreateOperationCommand : ICommand
{
    private readonly IOperationFactory _operationFactory;
    private readonly IOperationRepository _operationRepository;
    private readonly IBankAccountRepository _accountRepository;

    private OperationType Type { get; }

    private Guid AccountId { get; }

    private decimal Amount { get; }

    private Guid CategoryId { get; }

    private string? Description { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    public CreateOperationCommand(IOperationFactory operationFactory,
        IOperationRepository operationRepository,
        IBankAccountRepository accountRepository,
        OperationType type,
        Guid accountId,
        decimal amount,
        Guid categoryId,
        string? description = null)
    {
        _operationFactory = operationFactory;
        _operationRepository = operationRepository;
        _accountRepository = accountRepository;
        Type = type;
        AccountId = accountId;
        Amount = amount;
        CategoryId = categoryId;
        Description = description;
    }

    public void Execute()
    {
        var account = _accountRepository.GetById(AccountId);
        if (account == null) throw new InvalidOperationException();

        var money = new Money(Amount);
        Operation operation;

        if (Type == OperationType.Income)
            operation = _operationFactory.CreateIncomeOperation(AccountId, money, CategoryId, Description);
        else
            operation = _operationFactory.CreateExpenseOperation(AccountId, money, CategoryId, Description);

        _operationRepository.Add(operation);

        if (Type == OperationType.Income)
            account.AddIncome(money, new Category(CategoryId, CategoryType.Income, "Temp"), Description);
        else
            account.AddExpense(money, new Category(CategoryId, CategoryType.Expense, "Temp"), Description);

        _accountRepository.Update(account);
    }
}