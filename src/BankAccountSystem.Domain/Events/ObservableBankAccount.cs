using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.ValueObjects;

namespace BankAccountSystem.Domain.Events;

public class ObservableBankAccount : BankAccount
{
    private readonly List<IBalanceObserver> _balanceObservers = [];
    private readonly List<IOperationObserver> _operationObservers = [];

    /// <summary>
    /// .ctor
    /// </summary>
    public ObservableBankAccount(string name, Money initialBalance) : base(name, initialBalance) { }

    public void AddBalanceObserver(IBalanceObserver observer) => _balanceObservers.Add(observer);

    public void RemoveBalanceObserver(IBalanceObserver observer) => _balanceObservers.Remove(observer);

    public void AddOperationObserver(IOperationObserver observer) => _operationObservers.Add(observer);

    public void RemoveOperationObserver(IOperationObserver observer) => _operationObservers.Remove(observer);

    public override Operation AddIncome(Money amount, Category category, string? description = null)
    {
        var oldBalance = Balance;
        var operation = base.AddIncome(amount, category, description);

        NotifyBalanceChanged(oldBalance, Balance);
        NotifyOperationAdded(operation);

        if (amount.Amount >= 10000)
        {
            NotifyLargeOperation(operation);
        }

        return operation;
    }

    public override Operation AddExpense(Money amount, Category category, string? description = null)
    {
        var oldBalance = Balance;
        var operation = base.AddExpense(amount, category, description);

        NotifyBalanceChanged(oldBalance, Balance);
        NotifyOperationAdded(operation);

        if (amount.Amount >= 10000)
        {
            NotifyLargeOperation(operation);
        }

        return operation;
    }

    private void NotifyBalanceChanged(Money oldBalance, Money newBalance)
    {
        foreach (var observer in _balanceObservers)
        {
            observer.OnBalanceChanged(Id, oldBalance.Amount, newBalance.Amount);
        }
    }

    private void NotifyOperationAdded(Operation operation)
    {
        foreach (var observer in _operationObservers)
        {
            observer.OnOperationAdded(operation);
        }
    }

    private void NotifyLargeOperation(Operation operation)
    {
        foreach (var observer in _operationObservers)
        {
            observer.OnLargeOperation(operation);
        }
    }
}