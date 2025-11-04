using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.Enums;

namespace BankAccountSystem.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly Dictionary<Guid, Category> _categories = new();

    public Category? GetById(Guid id) => _categories.GetValueOrDefault(id);

    public IEnumerable<Category> GetAll() => _categories.Values;

    public void Add(Category category)
    {
        if (!_categories.TryAdd(category.Id, category))
            throw new InvalidOperationException();
    }

    public void Update(Category category)
    {
        if (!_categories.ContainsKey(category.Id))
            throw new InvalidOperationException();
        _categories[category.Id] = category;
    }

    public void Delete(Guid id) => _categories.Remove(id);

    public IEnumerable<Category> GetByType(CategoryType type) =>
        _categories.Values.Where(c => c.Type == type);

    public bool Exists(Guid id) => _categories.ContainsKey(id);
}