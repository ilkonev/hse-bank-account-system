using BankAccountSystem.Domain.Commons;

namespace BankAccountSystem.Application.Services;

public interface IAnalyticsService
{
    decimal GetIncomeExpenseDifference(DateTime start, DateTime end);

    Dictionary<string, decimal> GetCategorySummary(DateTime start, DateTime end);

    FinancialReport GetFinancialReport(DateTime start, DateTime end);
}