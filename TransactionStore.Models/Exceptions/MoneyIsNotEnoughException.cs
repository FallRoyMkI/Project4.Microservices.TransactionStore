namespace TransactionStore.Models.Exceptions;
public class MoneyIsNotEnoughException : Exception
{
    public MoneyIsNotEnoughException(string message) : base(message) { }
}