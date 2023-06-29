namespace TransactionStore.Models.Exceptions;
public class AccountNotExistException : Exception
{
    public AccountNotExistException(string message) : base(message) { }
}
