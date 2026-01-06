using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class StoreBookEntityTypeConfiguration : IEntityTypeConfiguration<StoreBook>
{
    public void Configure(EntityTypeBuilder<StoreBook> builder)
    {
        builder.HasKey(e => new { e.StoreId, e.Isbn13 }).HasName("PK__StoreBoo__183D88E1C80CF3CE");

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .IsUnicode(false)
            .HasColumnName("ISBN13");

        builder.HasOne(d => d.Isbn13Navigation).WithMany(p => p.StoreBooks)
            .HasForeignKey(d => d.Isbn13)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__StoreBook__ISBN1__45FE52CB");

        builder.HasOne(d => d.Store).WithMany(p => p.StoreBooks)
            .HasForeignKey(d => d.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__StoreBook__Store__450A2E92");
    }
}