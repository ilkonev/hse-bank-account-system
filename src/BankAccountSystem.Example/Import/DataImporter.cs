using BankAccountSystem.Application.Interfaces;

namespace BankAccountSystem.Example.Import;

public abstract class DataImporter
{
    private readonly IBankAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOperationRepository _operationRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    protected DataImporter(
        IBankAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _operationRepository = operationRepository;
    }

    public async Task ImportAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Import file not found", filePath);

        var data = await ReadFileAsync(filePath);
        var entities = ParseData(data);
        await SaveEntitiesAsync(entities);
    }

    protected virtual async Task<string> ReadFileAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }

    protected abstract ImportData ParseData(string data);

    protected virtual async Task SaveEntitiesAsync(ImportData entities)
    {
        foreach (var account in entities.Accounts)
        {
            if (!_accountRepository.Exists(account.Id))
                _accountRepository.Add(account);
        }

        foreach (var category in entities.Categories)
        {
            if (!_categoryRepository.Exists(category.Id))
                _categoryRepository.Add(category);
        }

        foreach (var operation in entities.Operations)
        {
            if (!_operationRepository.Exists(operation.Id))
                _operationRepository.Add(operation);
        }

        await Task.CompletedTask;
    }
}