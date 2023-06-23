using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NLog.Targets;
using TransactionStore.API.Validations;
using TransactionStore.Contracts;
using TransactionStore.Models.Dtos;
using TransactionStore.Models.Models;
using TransactionStore.TransactionsGenerator;
using ILogger = NLog.ILogger;

namespace TransactionStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly TransactionValidator _validatorTransaction;
        private readonly TransferTransactionValidator _validatorTransfer;
        public TransactionController(ITransactionManager transactionManager, IMapper mapper, ILogger logger, TransactionValidator validator)
        {
            _transactionManager = transactionManager;
            _mapper = mapper;
            _logger = logger;
            _validatorTransaction = validator;
        }

        [HttpPost("/")]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionDtoRequest transaction)
        {
            try
            {
                var validationResult = _validatorTransaction.Validate(transaction);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.Warn(error.ErrorMessage);
                    }

                    return BadRequest(validationResult.Errors);
                }

                Transaction transactionBll = _mapper.Map<Transaction>(transaction);
                int resultId = await _transactionManager.CreateTransactionAsync(transactionBll);

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
                if (accountId < 1)
                {
                    ArgumentException ex = new ArgumentException("Invalid accountId");
                    _logger.Warn(ex.Message);

                    return BadRequest(ex.Message);
                }

                decimal balance = await _transactionManager.GetAccountBalanceAsync(accountId);

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
                if (transactionId < 1)
                {
                    ArgumentException ex = new ArgumentException("Invalid transactionId");
                    _logger.Warn(ex.Message);

                    return BadRequest(ex.Message);
                }

                Transaction callback = await _transactionManager.GetTransactionByIdAsync(transactionId);
                TransactionDtoResponse transaction = _mapper.Map<TransactionDtoResponse>(callback);

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

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        public async Task Fill()
        {
            int startLeadId = 6102;
            int countLeads = 4000000;
            GeneratorContext _context = new();

            for (int i = startLeadId; i < startLeadId + countLeads; ++i)
            {
                List<Accounts> accounts = _context.Accounts.FromSql($"Exec GetAccountsByLeadId {i}").ToList();
                int countAccounts = accounts.Count();

                if (countAccounts == 1)
                {
                    await TransactionDeposit(accounts[0]);
                    await TransactionDeposit(accounts[0]);
                    await TransactionWithdraw(accounts[0]);
                }
                else
                {
                    foreach (var account in accounts)
                    {
                        if(account.Currency == "PY")
                        {
                            account.Currency = "JPY";
                        }
                    }

                    for (int j = 0; j < countAccounts * 0.7*3; ++j )
                    {
                        await TransactionTransfer(accounts);
                    }

                    for (int j = 0; j < countAccounts * 0.2 * 3; ++j)
                    {
                        int accountIndex = new Random().Next(countAccounts);
                        await TransactionDeposit(accounts[accountIndex]);
                    }

                    for (int j = 0; j < countAccounts * 0.1 * 3; ++j)
                    {
                        int accountIndex = new Random().Next(countAccounts);
                        await TransactionWithdraw(accounts[accountIndex]);
                    }
                }
            }
        }

        private async Task TransactionDeposit(Accounts accounts)
        {
            int amountDeposit = new Random().Next(1, 10000);
            Transaction transaction = new()
            {
                AccountId = accounts.Id,
                Amount = amountDeposit
            };
            await _transactionManager.CreateTransactionAsync(transaction);
        }

        private async Task TransactionWithdraw(Accounts accounts)
        {
            int amountWithdraw = new Random().Next(-10000, -1);
            Transaction transaction = new()
            {
                AccountId = accounts.Id,
                Amount = amountWithdraw
            };
            await _transactionManager.CreateTransactionAsync(transaction);
        }

        private async Task TransactionTransfer(List<Accounts> accounts)
        {
            int accountIndex = 0;
            int targetIndex = 0;
            int amount = 0;
            int countAccounts = accounts.Count();


            while (accountIndex == targetIndex)
            {
                accountIndex = new Random().Next(countAccounts);
                targetIndex = new Random().Next(countAccounts);
            }

            
            while (amount == 0)
            {
                amount = new Random().Next(1000);
            }

            TransferTransaction transaction = new()
            {
                AccountId = accounts[accountIndex].Id,
                TargetAccountId = accounts[targetIndex].Id,
                MoneyType = accounts[accountIndex].Currency,
                TargetMoneyType = accounts[targetIndex].Currency,
                Amount = amount
            };

            await _transactionManager.CreateTransferTransactionAsync(transaction);
        }
    }
}
