using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.Events;

namespace BankAccountSystem.Application.Services;

public class BalanceNotificationService : IBalanceObserver, IOperationObserver
{
    private readonly decimal _lowBalanceThreshold;
    private readonly decimal _largeOperationThreshold;

    /// <summary>
    /// .ctor
    /// </summary>
    public BalanceNotificationService(decimal lowBalanceThreshold = 1000, decimal largeOperationThreshold = 10000)
    {
        _lowBalanceThreshold = lowBalanceThreshold;
        _largeOperationThreshold = largeOperationThreshold;
    }

    public void OnBalanceChanged(Guid accountId, decimal oldBalance, decimal newBalance)
    {
        Console.WriteLine($"Баланс счета {accountId} изменился: {oldBalance} -> {newBalance}");

        if (newBalance < _lowBalanceThreshold)
        {
            OnLowBalance(accountId, newBalance);
        }
    }

    public void OnLowBalance(Guid accountId, decimal currentBalance)
    {
        Console.WriteLine($"Назкий баланс счета. Счет {accountId}: {currentBalance} руб.");
    }

    public void OnOperationAdded(Operation operation)
    {
        Console.WriteLine($"Добавлена операция: {operation.Type} на сумму {operation.Amount.Amount}");
    }

    public void OnLargeOperation(Operation operation)
    {
        if (operation.Amount.Amount >= _largeOperationThreshold)
        {
            Console.WriteLine($"Крупная операция: {operation.Type} на сумму {operation.Amount.Amount}");
        }
    }
}