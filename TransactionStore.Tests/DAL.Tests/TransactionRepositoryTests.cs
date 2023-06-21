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
        private Mock<ITransactionRepository> _mock;
        private Context _context;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }
        public async Task SetupAsync()
        {
            _context = new Context();
            _transactionRepository = new TransactionRepository(_context);
            _mock = new Mock<ITransactionRepository>();
        }

        [TestCaseSource(typeof(TransactionRepositoryTestCaseSource), nameof(TransactionRepositoryTestCaseSource.CreateTransactionAsyncTestCaseCource))]
        public async Task CreateTransactionAsyncTest(TransactionEntity transaction, int expectedId)
        {
            _mock.Setup( (ITransactionRepository o) => o.CreateTransactionAsync(transaction)).ReturnsAsync(transaction.Id).Verifiable();
            int expected = expectedId;
            int actual = await _transactionRepository.CreateTransactionAsync(transaction);
            _mock.VerifyAll();

            Assert.AreEqual(expected, actual);
        }
    }
}
