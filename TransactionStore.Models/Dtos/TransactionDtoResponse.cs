using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Dtos;

public class TransactionDtoResponse
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Type { get; set; }
    public int Amount { get; set; }
    public DateTime Time { get; set; }
}