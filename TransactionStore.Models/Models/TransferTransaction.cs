using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Models;

public class TransferTransaction
{
    public int AccountId { get; set; }
    public int Amount { get; set; }
    public string MoneyType { get; set; }
    public int TargetAccountId { get; set; }
    public string TargetMoneyType { get; set; }
}