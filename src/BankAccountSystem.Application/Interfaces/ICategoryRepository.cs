using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Domain.Enums;

namespace BankAccountSystem.Application.Interfaces;

public interface ICategoryRepository
{
    Category? GetById(Guid id);

    IEnumerable<Category> GetAll();

    void Add(Category category);

    void Update(Category category);

    void Delete(Guid id);

    IEnumerable<Category> GetByType(CategoryType type);

    bool Exists(Guid id);
}