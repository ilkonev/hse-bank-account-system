namespace BankAccountSystem.Domain.Events;

public interface IBalanceObserver
{
    void OnBalanceChanged(Guid accountId, decimal oldBalance, decimal newBalance);

    void OnLowBalance(Guid accountId, decimal currentBalance);
}