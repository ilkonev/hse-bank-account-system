namespace BankAccountSystem.Domain.Commons;

public class FinancialReport
{
    public decimal TotalIncome { get; set; }

    public decimal TotalExpenses { get; set; }

    public decimal Difference => TotalIncome - TotalExpenses;

    public Dictionary<string, decimal> IncomeByCategory { get; } = new();

    public Dictionary<string, decimal> ExpensesByCategory { get; } = new();
}