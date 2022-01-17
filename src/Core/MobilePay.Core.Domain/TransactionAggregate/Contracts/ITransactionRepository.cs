using MobilePay.Core.Domain.TransactionAggregate.Dto;
using static MobilePay.Core.Domain.TransactionAggregate.TransactionCommand;

namespace MobilePay.Core.Domain.TransactionAggregate.Contracts;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> CreateListAsync(CreateTransactionCommand transaction, CancellationToken cancellationToken = default);
    IQueryable<Transaction> GetMerchantTransactionsAsync(string merchantName, CancellationToken cancellationToken = default);
    Task<List<DayOfWeekTransactions>> GetDayOfWeekAsync(string merchantName, CancellationToken cancellationToken = default);
}
