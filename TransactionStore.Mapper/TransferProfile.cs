using AutoMapper;
using TransactionStore.Models.Dtos;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Models;

namespace TransactionStore.Mapper;

public class TransferProfile : Profile
{
    public TransferProfile()
    {
        CreateMap<TransactionDtoRequest, Transaction>();
        CreateMap<TransferTransactionDtoRequest, TransferTransaction>();
        CreateMap<Transaction, TransactionEntity>().ReverseMap();
        CreateMap<Transaction, TransactionDtoResponse>();
    }
}