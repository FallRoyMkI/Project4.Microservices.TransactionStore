using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Kerberos;
using TransactionStore.Contracts;
using TransactionStore.Models.Entities;

namespace TransactionStore.DAL;

public class TransactionRepository : ITransactionRepository
{
    private readonly Context _context;

    public TransactionRepository(Context context)
    {
        _context = context;
    }
    public async Task<int> CreateTransactionAsync(TransactionEntity transaction)
    {
        //хранимка на добавление в базу с установкой времени;
        var transactionId = _context.Database.SqlQuery<int>($"EXEC AddTransaction {transaction.AccountId}, {transaction.Type}, {transaction.Amount}").ToList();

        return transactionId[0];
    }

    public async Task<List<int>> CreateTransferTransactionAsync(TransactionEntity transferWithdraw, TransactionEntity transferDeposit)
    {
        var transactionId = await _context.Database.SqlQuery<int>
            ($"EXEC AddTransfer {transferWithdraw.AccountId}, {transferWithdraw.Type}, {transferWithdraw.Amount},{transferDeposit.AccountId}, {transferDeposit.Type}, {transferDeposit.Amount}").ToListAsync();

        return transactionId;
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        if (!await IsAccountExistInDbAsync(accountId)) return 0;

        List<TransactionEntity> accountOperations = (await _context.Transactions.ToListAsync()).FindAll(x => x.AccountId == accountId);

        return accountOperations.Sum(transaction => transaction.Amount);
    }

    public async Task<TransactionEntity> GetTransactionByIdAsync(int transactionId)
    {

        return await _context.Transactions.SingleAsync(x => x.Id == transactionId);
    }

    public async Task<List<TransactionEntity>> GetAllTransactionsByAccountIdAsync(int accountId)
    {
        return (await _context.Transactions.ToListAsync()).FindAll(x => x.AccountId == accountId);
    }

    private async Task<bool> IsAccountExistInDbAsync(int accountId)
    {
        return await _context.Transactions.AnyAsync(x=> x.AccountId == accountId);
    }
}