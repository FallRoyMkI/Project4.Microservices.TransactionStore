using System.Collections;
using TransactionStore.Models.Dtos;
using TransactionStore.Models.Models;

namespace TransactionStore.Tests.API.Tests;

public class TransactionControllerTestCaseSource
{
    public static IEnumerable CreateTransactionAsyncTestCaseSource()
    {
        TransactionDtoRequest transaction = new()
        {
            AccountId = 1,
            Amount = 111
        };
        Transaction entity = new()
        {
            AccountId = 1,
            Amount = 111,
        };
        int expectedId = 1;

        yield return new Object[] { transaction, entity, expectedId };
    }
}