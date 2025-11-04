using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Domain.Events;

public interface IOperationObserver
{
    void OnOperationAdded(Operation operation);

    void OnLargeOperation(Operation operation);
}