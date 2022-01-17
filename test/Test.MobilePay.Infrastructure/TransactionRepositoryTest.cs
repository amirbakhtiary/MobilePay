using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Core.Domain.TransactionAggregate.ValueObjects;
using MobilePay.Infrastructures.Data.Repositories;
using Test.MobilePay.Infrastructure.Initialize;
using Xunit;

namespace Test.MobilePay.Infrastructure;

public class TransactionRepositoryTest : DatabaseTestBase
{
    private readonly TransactionRepository _testee;
    public TransactionRepositoryTest()
    {
        _testee = new TransactionRepository(Context);
    }

    [Fact]
    public async void Transaction_CreateListAsync_Success()
    {
        var merchantList = MerchantData.GetMerchantList();

        var result = await _testee.CreateListAsync(new()
        {
            MerchantName = MerchantName.Create("tesla", merchantList),
            Merchants = merchantList,
            Transactions = new()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 10000,
                    Timestamp = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 5000,
                    Timestamp = DateTime.UtcNow
                }
            }
        });

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async void Transaction_GetMerchantTransanction()
    {
        var result = await _testee.GetDayOfWeekAsync("tesla");

        Assert.Equal(4, result.Count());
    }

    [Fact]
    public void Transaction_GetTransanctions()
    {
        var result = _testee.GetMerchantTransactionsAsync("tesla");

        Assert.Equal(4, result.Count());
    }
}
