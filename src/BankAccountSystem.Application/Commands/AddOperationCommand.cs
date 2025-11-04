using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Enums;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Application.Commands;

public class AddOperationCommand : ICommand
{
    private readonly IBankAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOperationRepository _operationRepository;

    private OperationType Type { get; }

    private Guid AccountId { get; }

    private decimal Amount { get; }

    private Guid CategoryId { get; }

    private string? Description { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    public AddOperationCommand(
        IBankAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository,
        OperationType type, Guid accountId, decimal amount, Guid categoryId, string? description = null)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _operationRepository = operationRepository;
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

        var category = _categoryRepository.GetById(CategoryId);
        if (category == null) throw new InvalidOperationException();

        var money = new Money(Amount);

        var operation = Type == OperationType.Income ?
            account.AddIncome(money, category, Description) :
            account.AddExpense(money, category, Description);

        _operationRepository.Add(operation);
        _accountRepository.Update(account);
    }
}