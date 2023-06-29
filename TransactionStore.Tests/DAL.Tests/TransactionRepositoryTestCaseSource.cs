using System.Collections;
using TransactionStore.Models.Entities;

namespace TransactionStore.Tests.DAL.Tests;
public class TransactionRepositoryTestCaseSource
{
    public static IEnumerable CreateTransactionAsyncTestCaseCource()
    {
        TransactionEntity transaction = new()
        {
            Id = 1,
            AccountId = 1,
            Amount = 111,
            Type = Models.Enums.TransactionType.Deposit,
            Time = new DateTime(2021, 01, 12)
        };

        int expectedId = transaction.Id;

        yield return new Object[] { transaction, expectedId };
    }
}