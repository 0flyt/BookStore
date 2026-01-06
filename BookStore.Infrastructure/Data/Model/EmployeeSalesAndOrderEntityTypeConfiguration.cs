using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class EmployeeSalesAndOrderEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeSalesAndOrder>
{
    public void Configure(EntityTypeBuilder<EmployeeSalesAndOrder> builder)
    {
        builder
            .HasNoKey()
            .ToView("EmployeeSalesAndOrders");

        builder.Property(e => e.Name).HasMaxLength(201);
        builder.Property(e => e.OfOrders).HasColumnName("# of orders");
        builder.Property(e => e.OfOrdersRecieved).HasColumnName("# of orders recieved");
        builder.Property(e => e.OfOrdersSend).HasColumnName("# of orders send");
        builder.Property(e => e.OfSales).HasColumnName("# of sales");
        builder.Property(e => e.OfSoldArticles).HasColumnName("# of sold articles");
        builder.Property(e => e.Store).HasMaxLength(100);
        builder.Property(e => e.TotalSalePriceInSwedishKr)
            .HasColumnType("decimal(38, 2)")
            .HasColumnName("Total sale price in swedish kr");
    }
}
