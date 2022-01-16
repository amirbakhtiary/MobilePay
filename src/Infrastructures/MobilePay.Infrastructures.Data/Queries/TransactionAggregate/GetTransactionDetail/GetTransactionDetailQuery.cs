using MediatR;

namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactionDetail;

public class GetTransactionDetailQuery : IRequest<GetTransactionDetailDto>
{
    public GetTransactionDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid? Id { get; }
}
