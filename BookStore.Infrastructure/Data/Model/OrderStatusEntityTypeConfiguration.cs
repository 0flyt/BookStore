using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.HasKey(e => e.StatusId).HasName("PK__OrderSta__C8EE206300EDE9F8");

        builder.ToTable("OrderStatus");

        builder.Property(e => e.StatusName).HasMaxLength(100);
    }
}
