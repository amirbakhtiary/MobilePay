using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobilePay.Core.Domain.TransactionAggregate;

namespace MobilePay.Infrastructures.Data.Configuration;

public class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(c => c.MerchantName, d =>
        {
            d.Property(e => e.Value)
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnName("MerchantName");
        });
    }
}
