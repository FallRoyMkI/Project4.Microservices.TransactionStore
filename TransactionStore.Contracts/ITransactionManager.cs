using TransactionStore.Models.Entities;
using TransactionStore.Models.Models;

namespace TransactionStore.Contracts;

public interface ITransactionManager
{
    public Task<int> CreateTransactionAsync(Transaction transactionRequest);
    public Task<List<int>> CreateTransferTransactionAsync(TransferTransaction transaction);
    public Task<decimal> GetAccountBalanceAsync(int accountId);
    public Task<Transaction> GetTransactionByIdAsync(int transactionId);
    public Task<List<Object>> GetAllTransactionsByAccountIdAsync(int accountId);
}