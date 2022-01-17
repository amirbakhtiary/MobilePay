using MobilePay.Core.Domain.TransactionAggregate.Dto;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;

public class GetMerchantFeeDto
{
    public GetMerchantFeeDto(decimal totalFee)
    {
        TotalFee = totalFee;
    }
    public decimal TotalFee { get; set; }
}
