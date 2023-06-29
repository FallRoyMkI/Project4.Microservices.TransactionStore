using AutoMapper;
using Moq;
using NUnit.Framework;
using TransactionStore.BLL;
using TransactionStore.Contracts;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Exceptions;
using TransactionStore.Models.Models;

namespace TransactionStore.Tests.BLL.Tests;
public class TransactionManagerTests
{
    private ITransactionManager _transactionManager;
    private Mock<ITransactionRepository> _mock;
    private Mock<IMapper> _mapperMock;
    private Mock<NLog.ILogger> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<ITransactionRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<NLog.ILogger>();
        _transactionManager = new TransactionManager(_mock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.CreateTransactionAsyncTestCaseSource))]
    public async Task CreateTransactionAsyncTests(Transaction transaction, TransactionEntity entity, int expectedId)
    {
        _mapperMock.Setup(o => o.Map<TransactionEntity>(transaction)).Returns(entity).Verifiable();
        _mock.Setup(o => o.CreateTransactionAsync(entity)).ReturnsAsync(expectedId).Verifiable();
        _mock.Setup(o => o.GetAccountBalanceAsync(transaction.AccountId)).ReturnsAsync(100);

        int actualId = await _transactionManager.CreateTransactionAsync(transaction);

        Assert.AreEqual(expectedId, actualId);
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.CreateTransactionAsyncNegativeTestCaseSource))]
    public void CreateTransactionAsyncTestWhenNotEnoughBalanceThenThrowMoneyIsNotEnoughException(Transaction transaction)
    {
        _mock.Setup(o => o.GetAccountBalanceAsync(transaction.AccountId)).ReturnsAsync(0);

        Assert.ThrowsAsync<MoneyIsNotEnoughException>(() => _transactionManager.CreateTransactionAsync(transaction));
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.CreateTransferTransactionAsyncTestCaseSource))]
    public async Task CreateTransferTransactionAsyncTests(TransferTransaction transaction, Transaction withdraw, Transaction deposit, TransactionEntity entityWithdraw, TransactionEntity entityDeposit, List<int> expectedIds)
    {
        _mapperMock.Setup(o => o.Map<TransactionEntity>(withdraw)).Returns(entityWithdraw).Verifiable();
        _mapperMock.Setup(o => o.Map<TransactionEntity>(deposit)).Returns(entityDeposit).Verifiable();

        _mock.Setup(o => o.GetAccountBalanceAsync(transaction.AccountId)).ReturnsAsync(10000);
        _mock.Setup(o => o.CreateTransferTransactionAsync(entityWithdraw, entityDeposit)).ReturnsAsync(expectedIds).Verifiable();

        List<int> actualIds = await _transactionManager.CreateTransferTransactionAsync(transaction);

        CollectionAssert.AreEqual(expectedIds, actualIds);
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.CreateTransferTransactionAsyncNegativeTestCaseSource))]
    public void CreateTransferTransactionAsyncTestWhenNotEnoughBalanceThenThrowMoneyIsNotEnoughException(TransferTransaction transaction)
    {
        _mock.Setup(o => o.GetAccountBalanceAsync(transaction.AccountId)).ReturnsAsync(0);

        Assert.ThrowsAsync<MoneyIsNotEnoughException>(() => _transactionManager.CreateTransferTransactionAsync(transaction));
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.GetAccountBalanceTestCaseSource))]
    public async Task GetAccountBalanceAsyncTests(int id, decimal expectedBalance)
    {
        _mock.Setup(o => o.GetAccountBalanceAsync(id)).ReturnsAsync(expectedBalance).Verifiable();

        decimal actualBalance = await _transactionManager.GetAccountBalanceAsync(id);

        Assert.AreEqual(expectedBalance, actualBalance);
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.GetTransactionByIdAsyncTestCaseSource))]
    public async Task GetTransactionByIdAsyncTests(int id, Transaction expectedTransaction, TransactionEntity entity)
    {
        _mapperMock.Setup(o => o.Map<Transaction>(entity)).Returns(expectedTransaction).Verifiable();
        _mock.Setup(o => o.GetTransactionByIdAsync(id)).ReturnsAsync(entity);

        Transaction actualTransaction = await _transactionManager.GetTransactionByIdAsync(id);

        Assert.AreEqual(expectedTransaction, actualTransaction);
    }

    [TestCaseSource(typeof(TransactionManagerTestCaseSource), nameof(TransactionManagerTestCaseSource.GetAllTransactionsByAccountIdAsyncTestCaseSource))]
    public async Task GetAllTransactionsByAccountIdAsyncTests(int id, List<TransactionEntity> entities, List<Transaction> transactions, List<Object> expectedTransactions)
    {
        _mapperMock.Setup(o => o.Map<List<Transaction>>(entities)).Returns(transactions).Verifiable();
        _mock.Setup(o => o.GetAllTransactionsByAccountIdAsync(id)).ReturnsAsync(entities);

        List<Object> actualTransactions = await _transactionManager.GetAllTransactionsByAccountIdAsync(id);

        CollectionAssert.AreEquivalent(expectedTransactions, actualTransactions);
    }
}