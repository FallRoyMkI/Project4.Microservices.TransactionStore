using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TransactionStore.API.Validations;
using TransactionStore.Contracts;
using TransactionStore.Models.Dtos;
using TransactionStore.Models.Models;
using ILogger = NLog.ILogger;

namespace TransactionStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionManager _transactionManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly TransactionValidator _validatorTransaction;
    private readonly TransferTransactionValidator _validatorTransfer;
    public TransactionController(ITransactionManager transactionManager, IMapper mapper, ILogger logger, TransactionValidator validator, TransferTransactionValidator validatorTransfer)
    {
        _transactionManager = transactionManager;
        _mapper = mapper;
        _logger = logger;
        _validatorTransaction = validator;
        _validatorTransfer = validatorTransfer;
    }

    [HttpPost("/")]
    public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionDtoRequest transaction)
    {
        try
        {
            _logger.Info($"Method was called with parameters: transaction = {JsonSerializer.Serialize(transaction)}");

            bool validationResult = _validatorTransaction.Validate(transaction).IsValid;
            if (!validationResult)
            {
                foreach (var error in _validatorTransaction.Validate(transaction).Errors)
                {
                    _logger.Warn(error.ErrorMessage);
                }

                return BadRequest(_validatorTransaction.Validate(transaction).Errors);
            }

            Transaction transactionBll = _mapper.Map<Transaction>(transaction);
            int resultId = await _transactionManager.CreateTransactionAsync(transactionBll);

            _logger.Info($"Method was finished with answer: transactionId = {resultId}");

            return Ok(resultId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/transfer")]
    public async Task<IActionResult> CreateTransferTransactionAsync([FromBody] TransferTransactionDtoRequest transferTransaction)
    {
        try
        {
            _logger.Info($"Method was called with parameters: transferTransaction = {JsonSerializer.Serialize(transferTransaction)}");

            var validationResult = _validatorTransfer.Validate(transferTransaction);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.Warn(error.ErrorMessage);
                }

                return BadRequest(validationResult.Errors);
            }

            TransferTransaction transactionBll = _mapper.Map<TransferTransaction>(transferTransaction);
            List<int> resultIds = await _transactionManager.CreateTransferTransactionAsync(transactionBll);

            _logger.Info($"Method was finished with answer: transactionsId = {resultIds[0]},{resultIds[1]}");

            return Ok(resultIds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/balanse/{accountId}")]
    public async Task<IActionResult> GetAccountBalanceAsync([FromRoute] int accountId)
    {
        try
        {
            _logger.Info($"Method was called with parameters: accountId = {accountId}");

            if (accountId < 1)
            {
                ArgumentException ex = new ArgumentException("Invalid accountId");
                _logger.Warn(ex.Message);

                return BadRequest(ex.Message);
            }

            decimal balance = await _transactionManager.GetAccountBalanceAsync(accountId);

            _logger.Info($"Method was finished with answer: balance = {balance}");

            return Ok(balance);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/transaction/{transactionId}")]
    public async Task<IActionResult> GetTransactionByIdAsync([FromRoute] int transactionId)
    {
        try
        {
            _logger.Info($"Method was called with parameters: transactionId = {transactionId}");

            if (transactionId < 1)
            {
                ArgumentException ex = new ArgumentException("Invalid transactionId");
                _logger.Warn(ex.Message);

                return BadRequest(ex.Message);
            }

            Transaction callback = await _transactionManager.GetTransactionByIdAsync(transactionId);
            TransactionDtoResponse transaction = _mapper.Map<TransactionDtoResponse>(callback);

            _logger.Info($"Method was finished with answer: transaction = {JsonSerializer.Serialize(transaction)}");

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/transactions/{accountId}")]
    public async Task<IActionResult> GetAllTransactionsByAccountIdAsync([FromRoute] int accountId)
    {
        try
        {
            _logger.Info($"Method was called with parameters: accountId = {accountId}");

            if (accountId < 1)
            {
                ArgumentException ex = new ArgumentException("Invalid accountId");
                _logger.Warn(ex.Message);

                return BadRequest(ex.Message);
            }

            List<Object> callback = await _transactionManager.GetAllTransactionsByAccountIdAsync(accountId);
            List<Object> result = new List<Object>();

            foreach (Object transaction in callback)
            {
                if (transaction is Transaction)
                {
                    result.Add(_mapper.Map<TransactionDtoResponse>(transaction));
                }
                else
                {
                    result.Add(_mapper.Map<TransactionDtoTransferResponse>(transaction));
                }
            }

            _logger.Info("Method was finished");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
