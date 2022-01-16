using MediatR;
using Microsoft.EntityFrameworkCore;
using MobilePay.Infrastructures.Data.Commons;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactionDetail;

public class GetTransactionDetailQueryHandler : IRequestHandler<GetTransactionDetailQuery, GetTransactionDetailDto>
{
    private readonly MobilePayDbContext _DbContext;

    public GetTransactionDetailQueryHandler(MobilePayDbContext dbContext)
    {
        _DbContext = dbContext;
    }

    public Task<GetTransactionDetailDto> Handle(GetTransactionDetailQuery request,
        CancellationToken cancellationToken) =>
        _DbContext.Transactions
        .Where(o => o.Id == request.Id)
        .Select(o => new GetTransactionDetailDto
        {
            Id = o.Id,
            MerchantName = o.MerchantName,
            Amount = o.Amount,
            Timestamp = o.Timestamp,
        })
        .AsNoTracking()
        .FirstOrDefaultAsync(cancellationToken);
}
