namespace MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;

public class CreateTransactionDto
{
    public Guid Id { get; set; }
    public string MerchantName { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}
