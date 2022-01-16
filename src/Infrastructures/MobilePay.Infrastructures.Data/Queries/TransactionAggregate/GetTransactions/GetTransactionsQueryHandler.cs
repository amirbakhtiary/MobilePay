using Framework.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MobilePay.Infrastructures.Data.Commons;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<GetTransactionsDto>>
{
    private readonly MobilePayDbContext _dbContext;

    public GetTransactionsQueryHandler(MobilePayDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<GetTransactionsDto>> Handle(GetTransactionsQuery request,
        CancellationToken cancellationToken) =>
        await _dbContext.Transactions
        .Where(t => t.MerchantName.Value.Equals(request.MerchantName))
        .Select(t => new GetTransactionsDto
        {
            Id = t.Id,
            MerchantName = t.MerchantName,
            Amount = t.Amount,
            Timestamp = t.Timestamp,
        })
        .AsNoTracking()
        .OrderByDescending(o => o.Timestamp)
        .ToPaging(request.Paging)
        .ToListAsync(cancellationToken);
}
