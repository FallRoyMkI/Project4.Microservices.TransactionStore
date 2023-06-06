using TransactionStore.Models.Models;

namespace TransactionStore.Contracts;

public interface ITransactionManager
{
    public Task<int> CreateTransactionAsync(Transaction transactionRequest);
    public Task<int[]> CreateTransferTransactionAsync(TransferTransaction transactionRequest);
    public Task<int> GetAccountBalanceAsync(int accountId);
    public Task<Transaction> GetTransactionByIdAsync(int transactionId);
}