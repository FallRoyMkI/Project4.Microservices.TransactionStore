using AutoMapper;
using Azure.Core.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TransactionStore.Contracts;
using TransactionStore.Models.Dtos;
using TransactionStore.Models.Models;

namespace TransactionStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionManager transactionManager, IMapper mapper)
        {
            _transactionManager = transactionManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionDtoRequest transaction)
        {
            // валидация на amount = 0;
            // валидация на account id >0;
            // валидация на account id => циферки а не буковки;

            Transaction transactionBll = _mapper.Map<Transaction>(transaction);
            int resultId = await _transactionManager.CreateTransactionAsync(transactionBll);

            return Ok(resultId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransferTransactionAsync([FromBody] TransferTransactionDtoRequest transferTransaction)
        {
            // валидация на amount = 0;
            // валидация на account id >0;
            // валидация на account id => циферки а не буковки;
            // валидация на baseMoney != null;


            TransferTransaction transactionBll = _mapper.Map<TransferTransaction>(transferTransaction);
            int[] resultIds = await _transactionManager.CreateTransferTransactionAsync(transactionBll);

            return Ok(resultIds);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountBalanceAsync([FromQuery] int accountId)
        {
            // валидация на account id >0;
            // валидация на account id => циферки а не буковки;

            int balance = await _transactionManager.GetAccountBalanceAsync(accountId);

            return Ok(balance);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionById([FromQuery] int transactionId)
        {
            {
                // валидация на id >0;
                // валидация на id => циферки а не буковки;

                Transaction callback = await _transactionManager.GetTransactionByIdAsync(transactionId);
                TransactionDtoResponse transaction = _mapper.Map<TransactionDtoResponse>(callback);

                return Ok(transaction);
            }
        }
    }
}