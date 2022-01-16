using Microsoft.EntityFrameworkCore;
using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Core.Domain.TransactionAggregate.Contracts;
using MobilePay.Core.Domain.TransactionAggregate.Dto;
using MobilePay.Infrastructures.Data.Commons;
using static MobilePay.Core.Domain.TransactionAggregate.TransactionCommand;

namespace MobilePay.Infrastructures.Data.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly MobilePayDbContext _dbContext;

    public TransactionRepository(MobilePayDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Transaction>> CreateListAsync(CreateTransactionCommand transaction, CancellationToken cancellationToken = default)
    {
        var transactionList = Transaction.CreateListAsync(transaction);
        await _dbContext.Transactions.AddRangeAsync(transactionList, cancellationToken);
        return transactionList;
    }

    public async Task<List<Transaction>> GetAllAsync(string merchantName,
        CancellationToken cancellationToken = default) =>
        await _dbContext.Transactions
        .Where(t => IsEqual(t.MerchantName.Value, merchantName))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    public async Task<List<DayOfWeekTransactions>> GetDayOfWeekAsync(string merchantName,
        CancellationToken cancellationToken = default) =>
        await _dbContext.Transactions
        .Where(t => IsEqual(t.MerchantName.Value, merchantName))
        .AsNoTracking()
        .GroupBy(t => new
        {
            Year = t.Timestamp.Year,
            Month = t.Timestamp.Month,
            DayOfWeek = GetDayOfWeek(t.Timestamp)
        })
        .Select(g => new DayOfWeekTransactions
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            DayOfWeek = g.Key.DayOfWeek,
            TotalCount = g.Count(),
            TotalAmount = g.Sum(t => t.Amount)
        })
        .ToListAsync(cancellationToken);

    private int GetDayOfWeek(DateTime dateTime) =>
        _dbContext.Database.IsRelational() ?
        EF.Functions.DateDiffDay(new DateTime(1753, 1, 7), dateTime) % 7 : (int)dateTime.DayOfWeek;

    private bool IsEqual(string val1, string val2) =>
         _dbContext.Database.IsRelational() ? val1.ToLower() == val2.ToLower() :
        val1.Equals(val2, StringComparison.OrdinalIgnoreCase);

}