using Framework.Tools;
using MediatR;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactions;

public class GetTransactionsQuery : IRequest<List<GetTransactionsDto>>
{
    public string MerchantName { get; set; }
    public Paging Paging { get; set; }
}
