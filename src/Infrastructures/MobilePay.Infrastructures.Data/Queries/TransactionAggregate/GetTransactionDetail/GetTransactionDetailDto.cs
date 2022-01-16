namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactionDetail
{
    public class GetTransactionDetailDto
    {
        public Guid Id { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
