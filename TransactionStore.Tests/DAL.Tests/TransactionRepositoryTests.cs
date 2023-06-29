using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using NUnit.Framework;
using TransactionStore.Contracts;
using TransactionStore.DAL;
using TransactionStore.Models.Entities;

namespace TransactionStore.Tests.DAL.Tests
{
    public class TransactionRepositoryTests
    {
        private ITransactionRepository _transactionRepository;
        private Mock<Context> _mock = new Mock<Context>();
        private Mock<Database> _mockDatabase;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<Context>();
            _transactionRepository = new TransactionRepository(_mock.Object, null);

        }

        [TestCaseSource(typeof(TransactionRepositoryTestCaseSource), nameof(TransactionRepositoryTestCaseSource.CreateTransactionAsyncTestCaseCource))]
        public async Task CreateTransactionAsyncTest(TransactionEntity transaction, int expectedId)
        {
            _mockDatabase.Setup(o => o.Database.SqlQuery<int>($"EXEC AddTransaction {transaction.AccountId}, {transaction.Type}, {transaction.Amount}")).Returns(new List<int>() { expectedId });
            int expected = expectedId;
            int actual = await _transactionRepository.CreateTransactionAsync(transaction);
            _mock.VerifyAll();

            Assert.AreEqual(expected, actual);
        }
    }
}
