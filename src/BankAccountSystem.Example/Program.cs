using BankAccountSystem.Application.Interfaces;
using BankAccountSystem.Application.Services;
using BankAccountSystem.Domain.Factory;
using BankAccountSystem.Example;
using BankAccountSystem.Example.Facades;
using BankAccountSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<BankAccountRepository>();
builder.Services.AddSingleton<IBankAccountRepository>(provider =>
    new CachedBankAccountRepository(provider.GetRequiredService<BankAccountRepository>()));
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IOperationRepository, OperationRepository>();
builder.Services.AddSingleton<IAnalyticsService, AnalyticsService>();
builder.Services.AddSingleton<IOperationFactory, OperationFactory>();
builder.Services.AddSingleton<BalanceNotificationService>();
builder.Services.AddSingleton<FinanceFacade>();
builder.Services.AddSingleton<Menu>();

var host = builder.Build();

try
{
    var mainMenu = host.Services.GetRequiredService<Menu>();
    mainMenu.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при запуске: {ex.Message}");
    Console.ReadKey();
}