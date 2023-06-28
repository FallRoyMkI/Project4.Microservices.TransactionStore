using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Kerberos;
using TransactionStore.Contracts;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Exceptions;
using TransactionStore.Models.Models;
using ILogger = NLog.ILogger;

namespace TransactionStore.DAL;

public class TransactionRepository : ITransactionRepository
{
    private readonly Context _context;
    private readonly ILogger _logger;
    public TransactionRepository(Context context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<int> CreateTransactionAsync(TransactionEntity transaction)
    {
        int? transactionId = await _context.Database.SqlQuery<int>($"EXEC AddTransaction {transaction.AccountId}, {transaction.Type}, {transaction.Amount}").SingleOrDefaultAsync();

        if (transactionId is null)
        {
            ServerException ex = new("Server response is null");
            _logger.Warn(ex.Message);

            throw ex;
        }

        return transactionId.Value;
    }

    public async Task<List<int>> CreateTransferTransactionAsync(TransactionEntity transferWithdraw, TransactionEntity transferDeposit)
    {
        List<int> transactionId = await _context.Database.SqlQuery<int>
            ($"EXEC AddTransfer {transferWithdraw.AccountId}, {transferWithdraw.Type}, {transferWithdraw.Amount},{transferDeposit.AccountId}, {transferDeposit.Type}, {transferDeposit.Amount}").ToListAsync();

        if (transactionId.Count() == 0)
        {
            ServerException ex = new("Server response is null");
            _logger.Warn(ex.Message);

            throw ex;
        }

        return transactionId;
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        if (!await IsAccountExistInDbAsync(accountId)) return 0;

        decimal balance = (await _context.Database.SqlQuery<decimal>($"EXEC GetAccountBalance {accountId}").ToListAsync())[0];

        return balance;
    }

    public async Task<TransactionEntity> GetTransactionByIdAsync(int transactionId)
    {
        TransactionEntity transaction = await _context.Transactions.SingleAsync(x => x.Id == transactionId);

        if (transaction is null)
        {
            ServerException ex = new("Server response is null");
            _logger.Warn(ex.Message);

            throw ex;
        }

        return transaction;
    }

    public async Task<List<TransactionEntity>> GetAllTransactionsByAccountIdAsync(int accountId)
    {
        if(!await IsAccountExistInDbAsync(accountId))
        {
            AccountNotExistException ex = new("Account not exist in DB");
            _logger.Warn(ex.Message);

            throw ex;
        }

        List<TransactionEntity> transactions = await _context.Transactions.FromSql($"EXEC GetTransactionsByAccountId {accountId}").ToListAsync();

        if (transactions.Count() == 0)
        {
            ServerException ex = new("Server response is null");
            _logger.Warn(ex.Message);

            throw ex;
        }

        return transactions;
    }

    private async Task<bool> IsAccountExistInDbAsync(int accountId)
    {
        return await _context.Transactions.AnyAsync(x=> x.AccountId == accountId);
    }
}