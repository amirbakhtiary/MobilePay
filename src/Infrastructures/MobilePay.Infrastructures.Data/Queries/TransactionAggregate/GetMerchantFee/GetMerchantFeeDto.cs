using MobilePay.Core.Domain.TransactionAggregate.Dto;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;

public class GetMerchantFeeDto
{
    public decimal TotalFee { get; set; }
}
