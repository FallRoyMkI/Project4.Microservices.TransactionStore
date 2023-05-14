using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.Contracts;

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
    }
}
