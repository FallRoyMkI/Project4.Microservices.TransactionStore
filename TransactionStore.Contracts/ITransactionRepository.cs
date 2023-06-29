using TransactionStore.Models.Entities;

namespace TransactionStore.Contracts;

public interface ITransactionRepository
{
    public Task<int> CreateTransactionAsync(TransactionEntity transaction);
    public Task<List<int>> CreateTransferTransactionAsync(TransactionEntity transferWithdraw, TransactionEntity transferDeposit);
    public Task<decimal> GetAccountBalanceAsync(int accountId);
    public Task<TransactionEntity> GetTransactionByIdAsync(int transactionId);
    public Task<List<TransactionEntity>> GetAllTransactionsByAccountIdAsync(int accountId);
}