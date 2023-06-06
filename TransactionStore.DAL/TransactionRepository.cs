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
        var transactionId = _context.Database.SqlQuery<int>($"DECLARE @newObjectId int\r\nEXEC @newObjectId = AddTransaction {transaction.AccountId}, {transaction.Type}, {transaction.Amount}\r\nSELECT @newObjectId AS Id").ToList();

        return transactionId[0];
    }

    public async Task<int[]> CreateTransferTransactionAsync(TransactionEntity transferWithdraw, TransactionEntity transferDeposit)
    {
        int[] ids = new int[2];
        await _context.Transactions.AddAsync(transferWithdraw);
        ids[1] = transferWithdraw.Id;
        await _context.Transactions.AddAsync(transferDeposit);
        ids[2] = transferDeposit.Id;

        return ids;
    }

    public async Task<int> GetAccountBalanceAsync(int accountId)
    {
        if (!await IsAccountExistInDbAsync(accountId)) return 0;

        List<TransactionEntity> accountOperations = (await _context.Transactions.ToListAsync()).FindAll(x => x.AccountId == accountId);

        return accountOperations.Sum(transaction => transaction.Amount);
    }

    public async Task<TransactionEntity> GetTransactionByIdAsync(int transactionId)
    {

        return await _context.Transactions.FindAsync(/*x => x.Id == transactionId*/);
    }

    private async Task<bool> IsAccountExistInDbAsync(int accountId)
    {
        return await _context.Transactions.AnyAsync(x=> x.AccountId == accountId);
    }
}