namespace MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactions
{
    public class GetTransactionsDto
    {
        public Guid Id { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
