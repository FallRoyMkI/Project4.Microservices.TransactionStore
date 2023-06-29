using System.Collections;
using TransactionStore.BLL;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Enums;
using TransactionStore.Models.Models;

namespace TransactionStore.Tests.BLL.Tests;
public class TransactionManagerTestCaseSource
{
    public static IEnumerable CreateTransactionAsyncTestCaseSource()
    {
        Transaction transaction = new()
        {
            AccountId = 1,
            Amount = 111
        };
        TransactionEntity entity = new TransactionEntity()
        {
            AccountId = 1,
            Amount = 111,
            Type = TransactionType.Deposit
        };
        int expectedId = 1;

        yield return new Object[] { transaction, entity, expectedId };

        transaction = new()
        {
            AccountId = 1,
            Amount = -10
        };
        entity = new TransactionEntity()
        {
            AccountId = 1,
            Amount = -10,
            Type = TransactionType.Withdraw
        };

        yield return new Object[] { transaction, entity, expectedId };
    }

    public static IEnumerable CreateTransactionAsyncNegativeTestCaseSource()
    {
        Transaction transaction = new()
        {
            AccountId = 1,
            Amount = -100
        };

        yield return new Object[] { transaction };
    }

    public static IEnumerable CreateTransferTransactionAsyncTestCaseSource()
    {
        CurrencyRate rate = new();
        TransferTransaction transaction = new()
        {
            AccountId = 1,
            Amount = 111,
            MoneyType = "RUB",
            TargetAccountId = 2,
            TargetMoneyType = "USD"
        };

        Transaction withdraw = new()
        {
            AccountId = 1,
            Type = TransactionType.TransferWithdraw,
            Amount = -111
        };
        Transaction deposit = new()
        {
            AccountId = 2,
            Type = TransactionType.TransferDeposit,
            Amount = 111 * rate.GetRate(transaction.MoneyType, transaction.TargetMoneyType)
        };

        TransactionEntity entityWithdraw = new TransactionEntity()
        {
            AccountId = 1,
            Type = TransactionType.TransferWithdraw,
            Amount = -111
        };
        TransactionEntity entityDeposit = new TransactionEntity()
        {
            AccountId = 2,
            Type = TransactionType.TransferDeposit,
            Amount = 111 * rate.GetRate(transaction.MoneyType, transaction.TargetMoneyType)
        };

        List<int> expectedIds = new() { 1, 2 };

        yield return new Object[] { transaction, withdraw, deposit, entityWithdraw, entityDeposit, expectedIds };
    }

    public static IEnumerable CreateTransferTransactionAsyncNegativeTestCaseSource()
    {
        TransferTransaction transaction = new()
        {
            AccountId = 1,
            Amount = 500,
            MoneyType = "RUB",
            TargetAccountId = 2,
            TargetMoneyType = "USD"
        };

        yield return new Object[] { transaction };
    }

    public static IEnumerable GetAccountBalanceTestCaseSource()
    {
        int id = 5;
        decimal balance = 100;
        yield return new Object[] { id, balance };
        id = 1;
        balance = -100;
        yield return new Object[] { id, balance };
    }

    public static IEnumerable GetTransactionByIdAsyncTestCaseSource()
    {
        int id = 5;
        Transaction transaction = new()
        {
            AccountId = 1,
            Amount = 111
        };
        TransactionEntity entity = new TransactionEntity()
        {
            AccountId = 1,
            Amount = 111,
            Type = TransactionType.Deposit
        };
        yield return new Object[] { id, transaction, entity };
        id = 2;
        transaction = new()
        {
            AccountId = 1,
            Amount = -10
        };
        entity = new TransactionEntity()
        {
            AccountId = 1,
            Amount = -10,
            Type = TransactionType.Withdraw
        };
        yield return new Object[] { id, transaction, entity };
    }

    public static IEnumerable GetAllTransactionsByAccountIdAsyncTestCaseSource()
    {
        int id = 5;
        Transaction withdraw = new()
        {
            Id = 3,
            AccountId = 5,
            Amount = -200,
            Time = new DateTime(2022, 11, 01, 15, 24, 33),
            Type = TransactionType.TransferWithdraw
        };
        Transaction deposit = new()
        {
            Id = 4,
            AccountId = 5,
            Amount = 100,
            Time = new DateTime(2022, 11, 01, 15, 24, 33),
            Type = TransactionType.TransferDeposit
        };

        List<Transaction> transactions = new()
            {
                new Transaction()
                {
                    Id = 1,
                    AccountId = 5,
                    Amount = -500,
                    Time = new DateTime(2022,11,27,15,24,00),
                    Type = TransactionType.Withdraw
                },
                new Transaction()
                {
                    Id = 2,
                    AccountId = 5,
                    Amount = 100,
                    Time = new DateTime(2022,10,27,15,24,00),
                    Type = TransactionType.Deposit
                },
                withdraw,
                deposit
            };
        List<TransactionEntity> entities = new()
            {
                new TransactionEntity()
                {
                    Id = 1,
                    AccountId = 5,
                    Amount = -500,
                    Time = new DateTime(2022,11,27,15,24,00),
                    Type = TransactionType.Withdraw
                },
                new TransactionEntity()
                {
                    Id = 2,
                    AccountId = 5,
                    Amount = 100,
                    Time = new DateTime(2022,10,27,15,24,00),
                    Type = TransactionType.Deposit
                },
                new TransactionEntity()
                {
                    Id = 3,
                    AccountId = 5,
                    Amount = -200,
                    Time = new DateTime(2022,11,01,15,24,33),
                    Type = TransactionType.TransferWithdraw
                },
                new TransactionEntity()
                {
                    Id = 4,
                    AccountId = 5,
                    Amount = 100,
                    Time = new DateTime(2022,11,01,15,24,33),
                    Type = TransactionType.TransferDeposit
                }
            };
        List<Object> result = new()
            {
                new Transaction()
                {
                    Id = 1,
                    AccountId = 5,
                    Amount = -500,
                    Time = new DateTime(2022,11,27,15,24,00),
                    Type = TransactionType.Withdraw
                },
                new Transaction()
                {
                    Id = 2,
                    AccountId = 5,
                    Amount = 100,
                    Time = new DateTime(2022,10,27,15,24,00),
                    Type = TransactionType.Deposit
                },
                new TransferTransactionResponse(withdraw,deposit)

            };
        yield return new Object[] { id, entities, transactions, result };
    }
}