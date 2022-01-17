using Framework.Tools;
using MobilePay.Core.Domain.Commons;
using MobilePay.Core.Domain.TransactionAggregate.Dto;
using MobilePay.Core.Domain.TransactionAggregate.ValueObjects;
using static MobilePay.Core.Domain.TransactionAggregate.TransactionCommand;

namespace MobilePay.Core.Domain.TransactionAggregate;

public class Transaction : AggregateRoot
{
    public MerchantName MerchantName { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Timestamp { get; private set; }

    public static IEnumerable<Transaction> CreateListAsync(CreateTransactionCommand detail) =>
        detail.Transactions.Select(o => new Transaction
        {
            Id = o.Id,
            MerchantName = MerchantName.Create(detail.MerchantName, detail.Merchants),
            Amount = o.Amount,
            Timestamp = o.Timestamp,
        });
    public static decimal CalculateMerchantFee(FeeSettingDto setting,
        IEnumerable<Transaction> transactions) =>
        CalculateMerchantFee(setting, MonthlyFeesExceptWeekend(transactions));

    public static decimal CalculateMerchantFee(FeeSettingDto setting,
        IEnumerable<DayOfWeekTransactions> dayOfWeekTransactions) =>
        CalculateMerchantFee(setting, MonthlyFeesExceptWeekend(dayOfWeekTransactions));

    private static decimal CalculateMerchantFee(FeeSettingDto setting,
        IEnumerable<MonthlyTransactions> monthlyTransactions)
    {
        decimal totalFee = 0;
        foreach (var monthlyFee in monthlyTransactions)
        {
            var fee = monthlyFee.TotalAmount.PercentCalc(setting.FeeSetting.FixedFee);
            fee -= fee.PercentCalc(setting.Merchant.Discount);

            if (monthlyFee.TotalCount.IfGreaterThan(setting.FeeSetting.VolumeCount))
                fee -= fee.PercentCalc(setting.FeeSetting.VolumeDiscount);

            totalFee += fee;
        }

        return totalFee;
    }

    private static IEnumerable<MonthlyTransactions> MonthlyFeesExceptWeekend(
        IEnumerable<DayOfWeekTransactions> dayOfWeekTransactions) =>
        dayOfWeekTransactions
        .Where(t => t.DayOfWeek.ExceptWeekend())
        .GroupBy(t => new
        {
            Year = t.Year,
            Month = t.Month,
        })
        .Select(g => new MonthlyTransactions
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            TotalCount = g.Sum(t => t.TotalCount),
            TotalAmount = g.Sum(t => t.TotalAmount)
        }).ToList();

    private static IEnumerable<MonthlyTransactions> MonthlyFeesExceptWeekend(
        IEnumerable<Transaction> transactions) =>
        transactions
        .Where(t => t.Timestamp.DayOfWeek.ExceptWeekend())
        .GroupBy(t => new
        {
            Year = t.Timestamp.Year,
            Month = t.Timestamp.Month,
        })
        .Select(g => new MonthlyTransactions
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            TotalCount = g.Count(),
            TotalAmount = g.Sum(t => t.Amount)
        }).ToList();
}