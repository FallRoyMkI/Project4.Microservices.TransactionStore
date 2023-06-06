namespace TransactionStore.Models.Dtos;

public class TransferTransactionDtoRequest
{
    public int AccountId { get; set; }
    public string MoneyType { get; set; }
    public int Amount { get; set; }
    public int TargetAccountId { get; set; }
    public string TargetMoneyType { get; set; }
}