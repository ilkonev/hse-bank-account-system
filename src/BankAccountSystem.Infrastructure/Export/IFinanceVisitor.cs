using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Infrastructure.Export;

public interface IFinanceVisitor
{
    void Visit(BankAccount account);

    void Visit(Category category);

    void Visit(Operation operation);

    string GetResult();
}