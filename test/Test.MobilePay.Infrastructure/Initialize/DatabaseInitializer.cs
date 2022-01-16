using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Infrastructures.Data.Commons;

namespace Test.MobilePay.Infrastructure.Initialize;

public class DatabaseInitializer
{
    public static void Initialize(MobilePayDbContext context)
    {
        if (!context.Transactions.Any())
            SeedTransactions(context);
    }

    private static void SeedTransactions(MobilePayDbContext context)
    {
        var transactions = Transaction.CreateListAsync(new()
        {
            MerchantName = "tesla",
            Merchants = MerchantData.GetMerchantList(),
            Transactions = new()
            {
                new()
                {
                    Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"),
                    Amount = 12000,
                    Timestamp = new DateTime(2022, 1, 12)
                },
                new()
                {
                    Id = Guid.Parse("B7F06A2B-DCF3-489E-9766-85456DC2797B"),
                    Amount = 26000,
                    Timestamp = new DateTime(2022, 1, 10)
                },
                new()
                {
                    Id = Guid.Parse("5854BA32-4B50-4A1A-8E1D-836E8CA719D6"),
                    Amount = 67500,
                    Timestamp = new DateTime(2022, 1, 11)
                },
                new()
                {
                    Id = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"),
                    Amount = 2370,
                    Timestamp = new DateTime(2022, 1, 9)
                }
            }
        });

        context.Transactions.AddRange(transactions);

        context.SaveChanges();
    }
}
