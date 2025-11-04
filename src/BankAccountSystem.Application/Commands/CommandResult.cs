namespace BankAccountSystem.Application.Commands;

public class CommandResult
{
    public bool IsSuccess { get; }

    public string Message { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    private CommandResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static CommandResult Success(string message)
        => new(true, message);

    public static CommandResult Failure(string message)
        => new(false, message);
}