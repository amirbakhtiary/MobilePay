using MediatR;

namespace MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;

public class CreateTransactionRequestCommand : IRequest<CreateTransactionRequestCommand>
{
    public string MerchantName { get; set; }
    public List<TransactionsDto> Transactions { get; set; }
}

public class TransactionsDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}
