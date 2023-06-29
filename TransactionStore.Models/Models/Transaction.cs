using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Models;

public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Time { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Transaction transaction &&
               Id == transaction.Id &&
               AccountId == transaction.AccountId &&
               Amount == transaction.Amount &&
               Type == transaction.Type &&
               Time == transaction.Time;
    }
}