namespace BankAccountSystem.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    private string Currency { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException();

        Amount = amount;
        Currency = "RUB";
    }

    public static Money operator +(Money left, Money right) =>
        new(left.Amount + right.Amount);

    public static Money operator -(Money left, Money right) =>
        new(left.Amount - right.Amount);

    public override string ToString() => $"{Amount:N2} {Currency}";
}