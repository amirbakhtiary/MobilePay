using AutoMapper;
using MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;
using MobilePay.Core.Domain.TransactionAggregate.Dto;
using MobilePay.Endpoints.WebApi.Models;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;
using static MobilePay.Core.Domain.TransactionAggregate.TransactionCommand;

namespace MobilePay.Endpoints.WebApi.Extensions.Mapper
{
    public class DataProfile : Profile
    {
        public DataProfile(MerchantList merchantList,
            FeeSetting feeSetting)
        {
            CreateMap<CreateTransactionRequestCommand, CreateTransactionCommand>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.MerchantName))
                .ForMember(dest => dest.Merchants, opt => opt.MapFrom(src => merchantList))
                .ForMember(dest => dest.Transactions, c => c.MapFrom(dto =>
                dto.Transactions.Select(o => new TransactionList
                {
                    Id = o.Id,
                    Amount = o.Amount,
                    Timestamp = o.Timestamp,
                })));

            CreateMap<AddTransactionModel, CreateTransactionRequestCommand>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.MerchantName))
                .ForMember(dest => dest.Transactions, c => c.MapFrom(o => new List<TransactionsDto>()
                {
                    new()
                    {
                        Id = o.Id,
                        Amount = o.Amount,
                        Timestamp = o.Timestamp,
                    }
                }));

            CreateMap<MerchantModel, GetMerchantFeeQuery>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.MerchantName))
                .ForMember(dest => dest.Merchants, opt => opt.MapFrom(src => merchantList))
                .ForMember(dest => dest.FeeSetting, opt => opt.MapFrom(src => feeSetting));

        }
    }
}
