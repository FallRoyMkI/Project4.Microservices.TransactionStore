namespace TransactionStore.Models.Dtos;

public class TransactionDtoRequest
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
}