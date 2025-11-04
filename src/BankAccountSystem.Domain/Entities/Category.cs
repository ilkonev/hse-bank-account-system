using BankAccountSystem.Domain.Enums;

namespace BankAccountSystem.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }

    public CategoryType Type { get; private set; }

    public string Name { get; private set; }

    /// <summary>
    /// .ctor
    /// </summary>
    public Category(CategoryType type, string name)
    {
        Id = Guid.NewGuid();
        Type = type;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// .ctor
    /// </summary>
    public Category(Guid id, CategoryType type, string name)
    {
        Id = id;
        Type = type;
        Name = name;
    }
}