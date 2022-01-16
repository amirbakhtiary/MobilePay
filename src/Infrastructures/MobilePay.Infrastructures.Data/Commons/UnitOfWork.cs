using MobilePay.Core.Domain.Commons;

namespace MobilePay.Infrastructures.Data.Commons;

public class UnitOfWork : IUnitOfWork
{
    private readonly MobilePayDbContext _dbContext;

    public UnitOfWork(MobilePayDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
