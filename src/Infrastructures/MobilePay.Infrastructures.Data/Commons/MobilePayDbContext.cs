using Microsoft.EntityFrameworkCore;
using MobilePay.Core.Domain.TransactionAggregate;

namespace MobilePay.Infrastructures.Data.Commons;

public class MobilePayDbContext : DbContext
{
    public MobilePayDbContext(DbContextOptions<MobilePayDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(500);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var changeCount = base.SaveChangesAsync(cancellationToken);
        return changeCount;
    }
}
