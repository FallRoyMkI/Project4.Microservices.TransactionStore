using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Dtos;

public class TransactionDtoResponse
{
    public int AccountId { get; set; }
    public TransactionType Type { get; set; }
    public int Amount { get; set; }
    public MoneyType BaseCurrency { get; set; }
    public int? LinkedAccountId { get; set; }
    public DateTime Time { get; set; }
}