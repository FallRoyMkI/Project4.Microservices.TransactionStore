using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Models;

public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public TransactionType Type { get; set; }
    public int Amount { get; set; }
    public MoneyType BaseCurrency { get; set; }
    public int? LinkedAccountId { get; set; }
    public DateTime Time { get; set; }
}