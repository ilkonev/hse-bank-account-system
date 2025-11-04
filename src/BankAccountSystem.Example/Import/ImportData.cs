using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Example.Import;

public class ImportData
{
    public List<BankAccount> Accounts { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Operation> Operations { get; set; } = new();
}