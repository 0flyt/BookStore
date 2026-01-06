using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class SaleEntityTypeConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C3FF1850F9FD");

        builder.Property(e => e.SaleDateTime).HasDefaultValueSql("(getdate())");

        builder.HasOne(d => d.Employee).WithMany(p => p.Sales)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Sales__EmployeeI__4AC307E8");

        builder.HasOne(d => d.Store).WithMany(p => p.Sales)
            .HasForeignKey(d => d.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Sales__StoreId__49CEE3AF");
    }
}
