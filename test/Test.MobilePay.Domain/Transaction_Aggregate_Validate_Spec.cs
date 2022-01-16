using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Core.Domain.TransactionAggregate.Dto;
using MobilePay.Core.Domain.TransactionAggregate.ValueObjects;
using static MobilePay.Core.Domain.Commons.DomainExceptions;
using static MobilePay.Core.Domain.TransactionAggregate.TransactionCommand;
using Xunit;
using Framework.Tools;

namespace Test.MobilePay.Domain;

public class Transaction_Aggregate_Validate_Spec
{
    [Fact]
    public void Transaction_MerchantName_Validate()
    {
        var merchantList = MerchantData.GetMerchantList();

        var merchantName = MerchantName.Create("tesla", merchantList);

        Assert.Equal("Tesla", merchantName.Value);
    }

    [Fact]
    public void Transaction_MerchantName_Throw_MerchantNameNotExistException()
    {
        var merchantList = MerchantData.GetMerchantList();

        Assert.Throws<MerchantNameNotExistException>(() => MerchantName.Create("ts", merchantList));
    }

    [Fact]
    public void Transaction_Should_Create_Successful()
    {
        var merchantList = MerchantData.GetMerchantList();

        var result = Transaction.CreateListAsync(new()
        {
            MerchantName = MerchantName.Create("Tesla", merchantList),
            Merchants = merchantList,
            Transactions = new()
            {
                new TransactionList
                {
                    Id = Guid.NewGuid(),
                    Amount = 10000,
                    Timestamp = DateTime.UtcNow
                },
                new TransactionList
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
    public void Transaction_CalculateMerchantFee_IsCorrect()
    {
        var merchant = new Merchant { Name = "Tesla", Discount = 25 };
        var setting = new FeeSettingDto { Merchant = merchant, FeeSetting = new FeeSetting() };
        var result = Transaction.CalculateMerchantFee(setting, GetDayOfWeekTransactions());

        Assert.Equal(150000, result);
    }

    [Theory]
    [InlineData(220000, 25, 55000)]
    [InlineData(250000, 25, 62500)]
    [InlineData(346000, 5, 17300)]
    [InlineData(126000, 15, 18900)]
    public void CalculateFee_PercentCalc_IsCorrect(decimal num, int discount, decimal result)
    {
        Assert.Equal(result, num.PercentCalc(discount));
    }

    [Theory]
    [InlineData(15, 10, true)]
    [InlineData(25, 10, true)]
    [InlineData(8, 10, false)]
    public void CalculateFee_IfGreaterThan_IsCorrect(int num, int count, bool result)
    {
        Assert.Equal(result, num.IfGreaterThan(count));
    }

    public List<DayOfWeekTransactions> GetDayOfWeekTransactions() =>
        new List<DayOfWeekTransactions>()
        {
            new()
            {
                Year = 2022,
                Month = 1,
                DayOfWeek = (int)DayOfWeek.Monday,
                TotalCount = 42,
                TotalAmount = 25000000
            }
        };
}
