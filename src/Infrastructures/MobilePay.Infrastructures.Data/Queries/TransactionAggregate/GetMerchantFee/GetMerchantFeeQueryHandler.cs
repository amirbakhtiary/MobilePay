using MediatR;
using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Core.Domain.TransactionAggregate.Contracts;
using MobilePay.Core.Domain.TransactionAggregate.Dto;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;

public class GetMerchantFeeQueryHandler : IRequestHandler<GetMerchantFeeQuery, GetMerchantFeeDto>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetMerchantFeeQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<GetMerchantFeeDto> Handle(GetMerchantFeeQuery request, CancellationToken cancellationToken)
    {
        Merchant merchant = request.Merchants.GetMerchant(request.MerchantName);
        var setting = new FeeSettingDto { Merchant = merchant, FeeSetting = request.FeeSetting };

        return new GetMerchantFeeDto(request.FeeSetting.CalculationMethod ?
            Transaction.CalculateMerchantFee(setting, await _transactionRepository
                .GetDayOfWeekAsync(request.MerchantName, cancellationToken)) :
            Transaction.CalculateMerchantFee(setting, _transactionRepository
                .GetMerchantTransactionsAsync(request.MerchantName, cancellationToken)));
    }
}
