
namespace TransactionStore.Models.Dtos;

public class TransactionDtoCreateTransferRequest
{
    public int AccountId { get; set; }
    public int Amount { get; set; }
    public int? LinkedAccountId { get; set; }
}