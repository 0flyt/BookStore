using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class FormatEntityTypeConfiguration : IEntityTypeConfiguration<Format>
{
    public void Configure(EntityTypeBuilder<Format> builder)
    {
        builder.HasKey(e => e.FormatId).HasName("PK__Formats__5D3DCB592EB8146D");

        builder.Property(e => e.Name).HasMaxLength(100);
    }
}
