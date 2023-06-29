using Moq;
using NUnit.Framework;
using TransactionStore.Contracts;
using TransactionStore.DAL;

namespace TransactionStore.Tests.DAL.Tests;

public class TransactionRepositoryTests
{
    private ITransactionRepository _transactionRepository;
    private Mock<Context> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<Context>();
        _transactionRepository = new TransactionRepository(_mock.Object, null);

    }

    //[TestCaseSource(typeof(TransactionRepositoryTestCaseSource), nameof(TransactionRepositoryTestCaseSource.CreateTransactionAsyncTestCaseCource))]
    //public async Task CreateTransactionAsyncTest(TransactionEntity transaction, int expectedId)
    //{
    //    _mock.Setup(o => o.Database.SqlQuery<int>($"EXEC AddTransaction {transaction.AccountId}, {transaction.Type}, {transaction.Amount}"))
    //        .Returns().Verifiable();
    //    int expected = expectedId;
    //    int actual = await _transactionRepository.CreateTransactionAsync(transaction);
    //    _mock.VerifyAll();

    //    Assert.AreEqual(expected, actual);
    //}
}
