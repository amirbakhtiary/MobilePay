using MediatR;
using MobilePay.Core.Domain.TransactionAggregate.Dto;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;

public class GetMerchantFeeQuery : IRequest<GetMerchantFeeDto>
{
    public string MerchantName { get; set; }
    public MerchantList Merchants { get; set; }
    public FeeSetting FeeSetting { get; set; }
}
