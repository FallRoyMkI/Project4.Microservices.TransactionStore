using AutoMapper;
using TransactionStore.Contracts;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Enums;
using TransactionStore.Models.Models;

namespace TransactionStore.BLL;

public class TransactionManager : ITransactionManager
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public TransactionManager(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateTransactionAsync(Transaction transaction)
    {
        transaction.Type = transaction.Amount < 0 ? TransactionType.Withdraw : TransactionType.Deposit;

        if (transaction.Type == TransactionType.Withdraw)
        {
            await IsEnoughMoneyForTransaction(transaction);
        }

        TransactionEntity transactionEntity = _mapper.Map<TransactionEntity>(transaction);
        int transactionId = await _transactionRepository.CreateTransactionAsync(transactionEntity);

        return transactionId;
    }

    public async Task<int[]> CreateTransferTransactionAsync(TransferTransaction transaction)
    {
        Transaction transferWithdraw = new Transaction()
        {
            AccountId = transaction.AccountId,
            Type = TransactionType.TransferWithdraw,
            Amount = transaction.Amount
        };

        await IsEnoughMoneyForTransaction(transferWithdraw);

        Transaction transferDeposit = new Transaction()
        {
            AccountId = transaction.AccountId,
            Type = TransactionType.TransferDeposit,
            // Amount = transaction.Amount * КОЭФФИЦИЕНТ
        };
        
        TransactionEntity transferWithdrawEntity = _mapper.Map<TransactionEntity>(transferWithdraw);
        TransactionEntity transferDepositEntity = _mapper.Map<TransactionEntity>(transferDeposit);
        int[] resultIds = await _transactionRepository.CreateTransferTransactionAsync(transferWithdrawEntity, transferDepositEntity);

        return resultIds;
    }

    public async Task<int> GetAccountBalanceAsync(int accountId)
    {
        return await _transactionRepository.GetAccountBalanceAsync(accountId);
    }

    public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
    {
        TransactionEntity callback = await _transactionRepository.GetTransactionByIdAsync(transactionId);
        Transaction transaction = _mapper.Map<Transaction>(callback);

        return transaction;
    }

    private async Task IsEnoughMoneyForTransaction(Transaction transaction)
    {
        int accountBalance = await _transactionRepository.GetAccountBalanceAsync(transaction.AccountId);

        if (accountBalance < Math.Abs(transaction.Amount))
        {
            throw new Exception("мала деняк");
        }
    }
}