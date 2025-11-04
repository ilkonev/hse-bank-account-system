using BankAccountSystem.Application.Commands;
using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Application.Services;
using BankAccountSystem.Domain.Enums;
using BankAccountSystem.Domain.Events;
using BankAccountSystem.Example.Facades;
using BankAccountSystem.Infrastructure.Repositories;

namespace BankAccountSystem.Example;

public class Menu
{
    private readonly FinanceFacade _facade;
    private readonly IBankAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOperationRepository _operationRepository;
    private readonly BalanceNotificationService _notificationService;

    /// <summary>
    /// .ctor
    /// </summary>
    public Menu(
        FinanceFacade facade,
        IBankAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository,
        BalanceNotificationService notificationService)
    {
        _facade = facade;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _operationRepository = operationRepository;
        _notificationService = notificationService;
    }

    public void Run()
    {
        Console.WriteLine("СИСТЕМА ФИНАНСОВОГО УЧЕТА\n");

        while (true)
        {
            DisplayMenu();
            var choice = GetMenuChoice();

            switch (choice)
            {
                case 1:
                    CreateAccount();
                    break;
                case 2:
                    CreateCategory();
                    break;
                case 3:
                    AddOperation();
                    break;
                case 4:
                    ShowAccounts();
                    break;
                case 5:
                    ShowFinancialReport();
                    break;
                case 6:
                    ExportData();
                    break;
                case 7:
                    ShowSystemDemo();
                    break;
                case 0:
                    Console.WriteLine("Выход из системы...");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("ГЛАВНОЕ МЕНЮ:");
        Console.WriteLine("1. Создать счет");
        Console.WriteLine("2. Создать категорию");
        Console.WriteLine("3. Добавить операцию");
        Console.WriteLine("4. Показать все счета");
        Console.WriteLine("5. Финансовый отчет");
        Console.WriteLine("6. Экспорт данных");
        Console.WriteLine("7. Демо системы");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите пункт меню: ");
    }

    private int GetMenuChoice()
    {
        if (int.TryParse(Console.ReadLine(), out int choice))
            return choice;
        return -1;
    }

    private void CreateAccount()
    {
        Console.WriteLine("\nСОЗДАНИЕ СЧЕТА");

        Console.Write("Введите название счета: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ошибка: Название не может быть пустым");
            return;
        }

        Console.Write("Введите начальный баланс: ");
        if (!decimal.TryParse(Console.ReadLine(), out var balance))
        {
            Console.WriteLine("Ошибка: Неверный формат баланса");
            return;
        }

        var command = new CreateAccountCommand(_accountRepository, name, balance);
        var result = _facade.ExecuteCommand(command, measureTime: true);

        Console.WriteLine(result.IsSuccess ? $"Счет успешно создан. {result.Message}" : $"Ошибка: {result.Message}");
    }

    private void CreateCategory()
    {
        Console.WriteLine("\nСОЗДАНИЕ КАТЕГОРИИ");

        Console.WriteLine("Выберите тип категории:");
        Console.WriteLine("1. Доход");
        Console.WriteLine("2. Расход");
        Console.Write("Ваш выбор: ");

        if (!int.TryParse(Console.ReadLine(), out var typeChoice) || (typeChoice != 1 && typeChoice != 2))
        {
            Console.WriteLine("Ошибка: Неверный выбор типа");
            return;
        }

        var categoryType = typeChoice == 1 ? CategoryType.Income : CategoryType.Expense;

        Console.Write("Введите название категории: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ошибка: Название не может быть пустым");
            return;
        }

        var command = new CreateCategoryCommand(_categoryRepository, categoryType, name);
        var result = _facade.ExecuteCommand(command);

        Console.WriteLine(result.IsSuccess
            ? $"Категория успешно создана. {result.Message}"
            : $"Ошибка: {result.Message}");
    }

    private void AddOperation()
    {
        Console.WriteLine("\nДОБАВЛЕНИЕ ОПЕРАЦИИ");

        var accounts = _facade.GetAccounts().ToList();
        if (accounts.Count == 0)
        {
            Console.WriteLine("Ошибка: Нет доступных счетов.");
            return;
        }

        Console.WriteLine("Доступные счета:");
        for (var i = 0; i < accounts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {accounts[i].Name} Баланс: {accounts[i].Balance}");
        }

        Console.Write("Выберите счет: ");
        if (!int.TryParse(Console.ReadLine(), out var accountIndex) || accountIndex < 1 || accountIndex > accounts.Count)
        {
            Console.WriteLine("Ошибка: Неверный выбор счета");
            return;
        }
        var selectedAccount = accounts[accountIndex - 1];

        Console.WriteLine("Тип операции:");
        Console.WriteLine("1. Доход");
        Console.WriteLine("2. Расход");
        Console.Write("Ваш выбор: ");

        if (!int.TryParse(Console.ReadLine(), out var operationTypeChoice) || (operationTypeChoice != 1 && operationTypeChoice != 2))
        {
            Console.WriteLine("Ошибка: Неверный выбор типа операции");
            return;
        }

        var operationType = operationTypeChoice == 1 ? OperationType.Income : OperationType.Expense;

        var categories = _facade.GetCategories()
            .Where(c => c.Type == (operationType == OperationType.Income ? CategoryType.Income : CategoryType.Expense))
            .ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("Ошибка: Нет доступных категорий для этого типа операции. Сначала создайте категорию.");
            return;
        }

        Console.WriteLine("Доступные категории:");
        for (var i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].Name}");
        }

        Console.Write("Выберите категорию: ");
        if (!int.TryParse(Console.ReadLine(), out var categoryIndex) || categoryIndex < 1 || categoryIndex > categories.Count)
        {
            Console.WriteLine("Ошибка: Неверный выбор категории");
            return;
        }
        var selectedCategory = categories[categoryIndex - 1];

        Console.Write("Введите сумму: ");
        if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
        {
            Console.WriteLine("Ошибка: Неверная сумма");
            return;
        }

        Console.Write("Введите описание: ");
        var description = Console.ReadLine();

        var command = new AddOperationCommand(
            _accountRepository, _categoryRepository, _operationRepository, operationType,
            selectedAccount.Id, amount, selectedCategory.Id, description);

        var result = _facade.ExecuteCommand(command, measureTime: true);

        Console.WriteLine(result.IsSuccess
            ? $"Операция успешно добавлена. {result.Message}"
            : $"Ошибка: {result.Message}");
    }

    private void ShowAccounts()
    {
        Console.WriteLine("\nВСЕ СЧЕТА");

        var accounts = _facade.GetAccounts().ToList();

        if (accounts.Count == 0)
        {
            Console.WriteLine("Счетов нет");
            return;
        }

        foreach (var account in accounts)
        {
            Console.WriteLine($"Счет: {account.Name}");
            Console.WriteLine($"  ID: {account.Id}");
            Console.WriteLine($"  Баланс: {account.Balance}");
            Console.WriteLine($"  Количество операций: {account.Operations.Count}");
            Console.WriteLine($"  Общие доходы: {account.GetTotalIncome()} руб.");
            Console.WriteLine($"  Общие расходы: {account.GetTotalExpenses()} руб.");

            if (account.Operations.Any())
            {
                Console.WriteLine("  Последние операции:");
                foreach (var operation in account.Operations.TakeLast(3))
                {
                    var category = _facade.GetCategories().FirstOrDefault(c => c.Id == operation.CategoryId);
                    Console.WriteLine($"    {operation.Date} - {operation.Type} - {operation.Amount} - {category?.Name}");
                }
            }
            Console.WriteLine();
        }
    }

    private void ShowFinancialReport()
    {
        Console.WriteLine("\nФИНАНСОВЫЙ ОТЧЕТ");

        var startDate = DateTime.Now.AddMonths(-1);
        var endDate = DateTime.Today + new TimeSpan(1, 0, 0, 0);

        var report = _facade.GetFinancialReport(startDate, endDate);

        Console.WriteLine($"Период: {startDate} - {endDate}");
        Console.WriteLine($"Общие доходы: {report.TotalIncome} руб.");
        Console.WriteLine($"Общие расходы: {report.TotalExpenses} руб.");
        Console.WriteLine($"Финансовый результат: {report.Difference} руб.");

        if (report.IncomeByCategory.Count != 0)
        {
            Console.WriteLine("Доходы по категориям:");
            foreach (var (category, amount) in report.IncomeByCategory)
            {
                Console.WriteLine($"  {category}: {amount} руб.");
            }
        }

        if (report.ExpensesByCategory.Count == 0) return;

        Console.WriteLine("Расходы по категориям:");
        foreach (var (category, amount) in report.ExpensesByCategory)
        {
            Console.WriteLine($"  {category}: {amount} руб.");
        }
    }

    private void ExportData()
    {
        Console.WriteLine("\nЭКСПОРТ ДАННЫХ");

        Console.WriteLine("Экспорт данных в JSON формате");

        var exportData = _facade.ExportData();

        var fileName = $"finance_export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        File.WriteAllText(fileName, exportData);

        Console.WriteLine($"Данные экспортированы в файл: {fileName}");
    }

    private void ShowSystemDemo()
    {
        Console.WriteLine("\nДЕМОНСТРАЦИЯ СИСТЕМЫ");

        Console.WriteLine("1. Демонстрация уведомлений");
        Console.Write("Выберите демо: ");

        if (!int.TryParse(Console.ReadLine(), out var choice)) return;

        switch (choice)
        {
            case 1:
                TestNotifications();
                break;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    private void TestNotifications()
    {
        Console.WriteLine("\nДЕМОНСТРАЦИЯ УВЕДОМЛЕНИЙ");

        var accounts = _facade.GetAccounts().ToList();
        if (accounts.Count == 0)
        {
            Console.WriteLine("Нет счетов для демонстрации");
            return;
        }

        var account = accounts.First();
        var categories = _facade.GetCategories().Where(c => c.Type == CategoryType.Expense).ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("Нет категорий расходов для демонстрации");
            return;
        }

        var category = categories.First();

        ObservableBankAccount observableAccount;
        if (account is ObservableBankAccount obsAccount)
        {
            observableAccount = obsAccount;
        }
        else
        {
            observableAccount = new ObservableBankAccount(account.Name, account.Balance);
            observableAccount.AddBalanceObserver(_notificationService);
            observableAccount.AddOperationObserver(_notificationService);
        }

        Console.WriteLine("Добавляем операцию для демонстрации уведомлений");
        var command = new AddOperationCommand(
            _accountRepository, _categoryRepository, _operationRepository,
            OperationType.Expense, observableAccount.Id, 15000, category.Id, "Демонстрация уведомлений");

        _facade.ExecuteCommand(command);
        Console.WriteLine("Демонстрация уведомлений завершена");
    }
}