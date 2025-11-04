using BankAccountSystem.Application.Commands;
using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Application.Services;
using BankAccountSystem.Domain.Commons;
using BankAccountSystem.Domain.Entities;
using BankAccountSystem.Example.Decorators;
using BankAccountSystem.Infrastructure.Export;

namespace BankAccountSystem.Example.Facades;

public class FinanceFacade
{
    private readonly IBankAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOperationRepository _operationRepository;
    private readonly IAnalyticsService _analyticsService;

    /// <summary>
    /// .ctor
    /// </summary>
    public FinanceFacade(
        IBankAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository,
        IAnalyticsService analyticsService)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _operationRepository = operationRepository;
        _analyticsService = analyticsService;
    }

    public CommandResult ExecuteCommand(ICommand command, bool measureTime = false)
    {
        try
        {
            var commandToExecute = measureTime
                ? new TimedCommandDecorator(command)
                : command;

            commandToExecute.Execute();

            if (commandToExecute is TimedCommandDecorator timedCommand)
            {
                return CommandResult.Success(
                    $"Команда успешно выполнена за {timedCommand.ExecutionTime.TotalMilliseconds} мс");
            }

            return CommandResult.Success("Команда успешно выполнена");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Ошибка: {ex.Message}");
        }
    }

    public string ExportData()
    {
        var visitor = new JsonExportVisitor();

        foreach (var account in _accountRepository.GetAll())
            visitor.Visit(account);

        foreach (var category in _categoryRepository.GetAll())
            visitor.Visit(category);

        foreach (var operation in _operationRepository.GetAll())
            visitor.Visit(operation);

        return visitor.GetResult();
    }

    public FinancialReport GetFinancialReport(DateTime start, DateTime end)
        => _analyticsService.GetFinancialReport(start, end);

    public IEnumerable<BankAccount> GetAccounts() => _accountRepository.GetAll();

    public IEnumerable<Category> GetCategories() => _categoryRepository.GetAll();
}