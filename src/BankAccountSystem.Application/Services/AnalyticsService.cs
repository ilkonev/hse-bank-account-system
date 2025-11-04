using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Domain.Commons;
using BankAccountSystem.Domain.Enums;

namespace BankAccountSystem.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IOperationRepository _operationRepository;
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// .ctor
    /// </summary>
    public AnalyticsService(IOperationRepository operationRepository, ICategoryRepository categoryRepository)
    {
        _operationRepository = operationRepository;
        _categoryRepository = categoryRepository;
    }

    public decimal GetIncomeExpenseDifference(DateTime start, DateTime end)
    {
        var operations = _operationRepository.GetByDateRange(start, end).ToList();
        var income = operations.ToList().Where(o => o.Type == OperationType.Income).Sum(o => o.Amount.Amount);
        var expenses = operations.Where(o => o.Type == OperationType.Expense).Sum(o => o.Amount.Amount);
        return income - expenses;
    }

    public Dictionary<string, decimal> GetCategorySummary(DateTime start, DateTime end)
    {
        var operations = _operationRepository.GetByDateRange(start, end);
        var categories = _categoryRepository.GetAll().ToDictionary(c => c.Id, c => c.Name);

        return operations.GroupBy(o => categories.GetValueOrDefault(o.CategoryId, "-"))
            .ToDictionary(g => g.Key, g => g.Sum(o => o.Amount.Amount));
    }

    public FinancialReport GetFinancialReport(DateTime start, DateTime end)
    {
        var operations = _operationRepository.GetByDateRange(start, end);
        var categories = _categoryRepository.GetAll().ToDictionary(c => c.Id, c => c);

        var report = new FinancialReport();

        foreach (var operation in operations)
        {
            var categoryName = categories.GetValueOrDefault(operation.CategoryId)?.Name ?? "-";

            if (operation.Type == OperationType.Income)
            {
                report.TotalIncome += operation.Amount.Amount;
                report.IncomeByCategory[categoryName] =
                    report.IncomeByCategory.GetValueOrDefault(categoryName) + operation.Amount.Amount;
            }
            else
            {
                report.TotalExpenses += operation.Amount.Amount;
                report.ExpensesByCategory[categoryName] =
                    report.ExpensesByCategory.GetValueOrDefault(categoryName) + operation.Amount.Amount;
            }
        }

        return report;
    }
}