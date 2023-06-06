using TransactionStore.Models.Entities;
using TransactionStore.Models.Models;

namespace TransactionStore.Contracts;

public interface ITransactionRepository
{
    public Task<int> CreateTransactionAsync(TransactionEntity transaction);
    public Task<int[]> CreateTransferTransactionAsync(TransactionEntity transactionWithdraw, TransactionEntity transactionDeposit);
    public Task<int> GetAccountBalanceAsync(int accountId);
    public Task<TransactionEntity> GetTransactionByIdAsync(int transactionId);

}