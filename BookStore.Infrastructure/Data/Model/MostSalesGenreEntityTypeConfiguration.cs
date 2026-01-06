using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class MostSalesGenreEntityTypeConfiguration : IEntityTypeConfiguration<MostSalesGenre>
{
    public void Configure(EntityTypeBuilder<MostSalesGenre> builder)
    {
        builder
            .HasNoKey()
            .ToView("MostSalesGenres");

        builder.Property(e => e.Genre).HasMaxLength(100);
        builder.Property(e => e.OfBooks).HasColumnName("# of books");
        builder.Property(e => e.OfSales).HasColumnName("# of sales");
    }
}
