namespace TransactionStore.Models.Dtos;

public class TransactionDtoResponse
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime Time { get; set; }
}