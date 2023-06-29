using AutoMapper;
using Moq;
using NUnit.Framework;
using TransactionStore.API.Controllers;
using TransactionStore.API.Validations;
using TransactionStore.Contracts;

namespace TransactionStore.Tests.API.Tests;

public class TransactionControllerTests
{
    private TransactionController _controller;
    private Mock<ITransactionManager> _mock;
    private Mock<IMapper> _mapperMock;
    private Mock<NLog.ILogger> _loggerMock;
    private Mock<TransferTransactionValidator> _transferValidatorMock;
    private Mock<TransactionValidator> _transactionValidatorMock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<ITransactionManager>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<NLog.ILogger>();
        _transactionValidatorMock = new Mock<TransactionValidator>();
        _transferValidatorMock = new Mock<TransferTransactionValidator>();
        _controller = new(_mock.Object, _mapperMock.Object, _loggerMock.Object, _transactionValidatorMock.Object, _transferValidatorMock.Object);
    }

    //[TestCaseSource(typeof(TransactionControllerTestCaseSource), nameof(TransactionControllerTestCaseSource.CreateTransactionAsyncTestCaseSource))]
    //public async Task CreateTransactionAsyncTests(TransactionDtoRequest dto, Transaction transaction, int expectedId)
    //{
    //    _mapperMock.Setup(o => o.Map<Transaction>(dto)).Returns(transaction).Verifiable();
    //    _mock.Setup(o => o.CreateTransactionAsync(kek)).ReturnsAsync(id).Verifiable();
    //    _transactionValidatorMock.Setup(o => o.Validate(dto).IsValid).Returns(true).Verifiable();

    //    IActionResult actual = await _controller.CreateTransactionAsync(dto);
    //    IActionResult expected = new OkObjectResult(id);

    //    Assert.AreEqual(actual,expected);
    //}
}