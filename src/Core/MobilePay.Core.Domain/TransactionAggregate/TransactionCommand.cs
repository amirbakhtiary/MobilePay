using MobilePay.Core.Domain.TransactionAggregate.Dto;

namespace MobilePay.Core.Domain.TransactionAggregate;

public static class TransactionCommand
{
    public class CreateTransactionCommand
    {
        public string MerchantName { get; set; }
        public MerchantList Merchants { get; set; }
        public List<TransactionList> Transactions { get; set; }
    }

    public class TransactionList
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
