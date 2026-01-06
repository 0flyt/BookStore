using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class SaleItemEntityTypeConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(e => new { e.SaleId, e.Isbn13 }).HasName("PK__SaleItem__3D5CBA1F31CFDB87");

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .IsUnicode(false)
            .HasColumnName("ISBN13");
        builder.Property(e => e.Quantity).HasDefaultValue(1);
        builder.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");

        builder.HasOne(d => d.Isbn13Navigation).WithMany(p => p.SaleItems)
            .HasForeignKey(d => d.Isbn13)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__SaleItems__ISBN1__4F87BD05");

        builder.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
            .HasForeignKey(d => d.SaleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__SaleItems__SaleI__4E9398CC");
    }
}
