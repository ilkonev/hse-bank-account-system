using BankAccountSystem.Application.Commands;

namespace BankAccountSystem.Example.Decorators;

public class TimedCommandDecorator : ICommand
{
    private readonly ICommand _decoratedCommand;
    public TimeSpan ExecutionTime { get; private set; }

    /// <summary>
    /// .ctor
    /// </summary>
    public TimedCommandDecorator(ICommand decoratedCommand)
    {
        _decoratedCommand = decoratedCommand;
    }

    public void Execute()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _decoratedCommand.Execute();
        stopwatch.Stop();
        ExecutionTime = stopwatch.Elapsed;
    }
}