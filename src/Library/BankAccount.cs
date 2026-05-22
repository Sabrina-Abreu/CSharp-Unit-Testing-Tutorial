namespace Library;

public class InsufficientFundsException : Exception
{
    public decimal Amount { get; }
    public InsufficientFundsException(decimal amount)
        : base($"Insufficient funds. Attempted to withdraw {amount:C}.") => Amount = amount;
}

/// <summary>
/// Bank account used to demonstrate exception testing.
/// </summary>
public class BankAccount
{
    public string Owner { get; }
    public decimal Balance { get; private set; }
    public List<string> TransactionHistory { get; } = [];

    public BankAccount(string owner, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Owner name is required.", nameof(owner));
        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance cannot be negative.");
        Owner = owner;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive.");
        Balance += amount;
        TransactionHistory.Add($"Deposit: +{amount:C} | Balance: {Balance:C}");
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be positive.");
        if (amount > Balance)
            throw new InsufficientFundsException(amount);
        Balance -= amount;
        TransactionHistory.Add($"Withdraw: -{amount:C} | Balance: {Balance:C}");
    }
}
