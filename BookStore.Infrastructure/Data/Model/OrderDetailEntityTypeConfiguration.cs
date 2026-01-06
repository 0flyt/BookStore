using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => new { e.OrderId, e.Isbn13 }).HasName("PK__OrderDet__E02F222FCD47932F");

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .IsUnicode(false)
            .HasColumnName("ISBN13");
        builder.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

        builder.HasOne(d => d.Isbn13Navigation).WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.Isbn13)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__OrderDeta__ISBN1__6576FE24");

        builder.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__OrderDeta__Order__6482D9EB");
    }
}
