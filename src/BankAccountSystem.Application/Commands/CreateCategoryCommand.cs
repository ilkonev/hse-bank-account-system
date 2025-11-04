using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.Enums;

namespace BankAccountSystem.Application.Commands;

public class CreateCategoryCommand : ICommand
{
    private readonly ICategoryRepository _categoryRepository;

    private CategoryType Type { get; }

    private string Name { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    public CreateCategoryCommand(ICategoryRepository categoryRepository, CategoryType type, string name)
    {
        _categoryRepository = categoryRepository;
        Type = type;
        Name = name;
    }

    public void Execute()
    {
        var category = new Category(Type, Name);
        _categoryRepository.Add(category);
    }
}