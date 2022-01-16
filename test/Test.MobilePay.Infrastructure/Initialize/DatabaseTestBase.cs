using Microsoft.EntityFrameworkCore;
using MobilePay.Core.Domain.Commons;
using MobilePay.Infrastructures.Data.Commons;

namespace Test.MobilePay.Infrastructure.Initialize;

public class DatabaseTestBase : IDisposable
{
    protected readonly MobilePayDbContext Context;
    protected readonly IUnitOfWork UnitOfWork;

    public DatabaseTestBase()
    {
        var options = new DbContextOptionsBuilder<MobilePayDbContext>()
            .UseInMemoryDatabase("Test")
            .Options;

        Context = new MobilePayDbContext(options);
        UnitOfWork = new UnitOfWork(Context);
        Context.Database.EnsureCreated();

        DatabaseInitializer.Initialize(Context);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
