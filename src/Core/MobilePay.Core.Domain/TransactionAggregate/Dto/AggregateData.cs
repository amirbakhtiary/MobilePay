namespace MobilePay.Core.Domain.TransactionAggregate.Dto;

public class MonthlyTransactions
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalCount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class DayOfWeekTransactions : MonthlyTransactions
{
    public int DayOfWeek { get; set; }
}