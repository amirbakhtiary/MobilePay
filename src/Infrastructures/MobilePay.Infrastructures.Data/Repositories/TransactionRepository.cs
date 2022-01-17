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

    public IQueryable<Transaction> GetMerchantTransactionsAsync(string merchantName,
        CancellationToken cancellationToken = default) =>
        _dbContext.Transactions
        .Where(t => _dbContext.Database.IsRelational() ?
            t.MerchantName.Value.Equals(merchantName) :
            t.MerchantName.Value.Equals(merchantName, StringComparison.OrdinalIgnoreCase))
        .AsNoTracking();

    public async Task<List<DayOfWeekTransactions>> GetDayOfWeekAsync(string merchantName,
        CancellationToken cancellationToken = default) =>
        await GetMerchantTransactionsAsync(merchantName, cancellationToken)
        .GroupBy(t => new
        {
            Year = t.Timestamp.Year,
            Month = t.Timestamp.Month,
            DayOfWeek = _dbContext.Database.IsRelational() ?
            EF.Functions.DateDiffDay(new DateTime(1753, 1, 7), t.Timestamp) % 7 :
            (int)t.Timestamp.DayOfWeek
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
}