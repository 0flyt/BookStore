using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class AuthorStatisticEntityTypeConfiguration : IEntityTypeConfiguration<AuthorStatistic>
{
    public void Configure(EntityTypeBuilder<AuthorStatistic> builder)
    {
        builder
            .HasNoKey()
            .ToView("AuthorStatistics");

        builder.Property(e => e.Age)
            .HasMaxLength(15)
            .IsUnicode(false);
        builder.Property(e => e.Name).HasMaxLength(201);
        builder.Property(e => e.Titles)
            .HasMaxLength(15)
            .IsUnicode(false);
        builder.Property(e => e.TotalInventoryValue)
            .HasMaxLength(44)
            .IsUnicode(false);
    }
}
