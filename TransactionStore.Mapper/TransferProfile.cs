using AutoMapper;
using TransactionStore.Models.Dtos;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Enums;
using TransactionStore.Models.Models;

namespace TransactionStore.Mapper;

public class TransferProfile : Profile
{
    public TransferProfile()
    {
        CreateMap<TransactionDtoRequest, Transaction>();
        CreateMap<TransferTransactionDtoRequest, TransferTransaction>();
        CreateMap<Transaction, TransactionEntity>().ReverseMap();
        CreateMap<Transaction, TransactionDtoResponse>().ForMember(x => x.Type, opt => opt.MapFrom(y => ((TransactionType)y.Type).ToString()));
        CreateMap<TransferTransactionResponse, TransactionDtoTransferResponse>().ForMember(x => x.Type, opt => opt.MapFrom(y => ((TransactionType)y.Type).ToString()));
    }
}