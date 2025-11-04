using BankAccountSystem.Domain.Entities;
using System.Text.Json;

namespace BankAccountSystem.Infrastructure.Export;

public class JsonExportVisitor : IFinanceVisitor
{
    private readonly List<object> _entities = [];

    public void Visit(BankAccount account)
    {
        _entities.Add(new
        {
            account.Id,
            account.Name,
            account.Balance.Amount
        });
    }

    public void Visit(Category category)
    {
        _entities.Add(new
        {
            category.Id,
            category.Type,
            category.Name
        });
    }

    public void Visit(Operation operation)
    {
        _entities.Add(new
        {
            operation.Id,
            operation.Type,
            operation.BankAccountId,
            operation.Amount.Amount,
            operation.Date,
            operation.Description,
            operation.CategoryId
        });
    }

    public string GetResult() => JsonSerializer.Serialize(_entities, new JsonSerializerOptions { WriteIndented = true });
}