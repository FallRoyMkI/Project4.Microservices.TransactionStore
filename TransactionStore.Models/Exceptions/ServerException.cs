namespace TransactionStore.Models.Exceptions;
public class ServerException : Exception
{
    public ServerException(string message) : base(message) { }
}