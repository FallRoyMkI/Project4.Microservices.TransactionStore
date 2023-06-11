using TransactionStore.Models.Models;

namespace TransactionStore.Contracts;

public interface ITransactionManager
{
    public Task<int> CreateTransactionAsync(Transaction transactionRequest);
    public Task<List<int>> CreateTransferTransactionAsync(TransferTransaction transaction);
    public Task<int> GetAccountBalanceAsync(int accountId);
    public Task<Transaction> GetTransactionByIdAsync(int transactionId);
    public Task<List<Transaction>> GetAllTransactionsByAccountIdAsync(int accountId);
}