using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Entities;

public class TransactionEntity
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
}