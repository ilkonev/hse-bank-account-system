using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Entities;

namespace BankAccountSystem.Example.Import;

public class JsonDataImporter : DataImporter
{
    /// <summary>
    /// .ctor
    /// </summary>
    public JsonDataImporter(
        IBankAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository)
        : base(accountRepository, categoryRepository, operationRepository)
    {
    }

    protected override ImportData ParseData(string data)
    {
        var importData = new ImportData();

        try
        {
            var jsonElements = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement[]>(data);

            foreach (var element in jsonElements ?? Array.Empty<System.Text.Json.JsonElement>())
            {
                if (element.TryGetProperty("Type", out var typeProperty))
                {
                    var type = typeProperty.GetString();
                    switch (type)
                    {
                        case "BankAccount":
                            var account = System.Text.Json.JsonSerializer.Deserialize<BankAccount>(
                                element.GetRawText());
                            if (account != null) importData.Accounts.Add(account);
                            break;
                        case "Category":
                            var category = System.Text.Json.JsonSerializer.Deserialize<Category>(
                                element.GetRawText());
                            if (category != null) importData.Categories.Add(category);
                            break;
                        case "Operation":
                            var operation = System.Text.Json.JsonSerializer.Deserialize<Operation>(
                                element.GetRawText());
                            if (operation != null) importData.Operations.Add(operation);
                            break;
                    }
                }
            }
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidOperationException("Invalid JSON format", ex);
        }

        return importData;
    }
}