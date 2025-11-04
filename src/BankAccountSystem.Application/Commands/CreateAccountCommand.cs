using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Application.Commands;

public class CreateAccountCommand : ICommand
{
    private readonly IBankAccountRepository _accountRepository;

    private string Name { get; }

    private decimal InitialBalance { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    public CreateAccountCommand(IBankAccountRepository accountRepository, string name, decimal initialBalance = 0)
    {
        _accountRepository = accountRepository;
        Name = name;
        InitialBalance = initialBalance;
    }

    public void Execute()
    {
        var account = new BankAccount(Name, new Money(InitialBalance));
        _accountRepository.Add(account);

    }
}