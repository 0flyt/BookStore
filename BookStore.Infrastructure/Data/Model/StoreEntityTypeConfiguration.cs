using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(e => e.StoreId).HasName("PK__Stores__3B82F101DA8C945A");

        builder.Property(e => e.Address).HasMaxLength(100);
        builder.Property(e => e.City).HasMaxLength(100);
        builder.Property(e => e.Country).HasMaxLength(100);
        builder.Property(e => e.StoreName).HasMaxLength(100);
    }
}
