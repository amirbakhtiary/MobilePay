namespace MobilePay.Core.Domain.Commons;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken = default);
    void Save();
}
