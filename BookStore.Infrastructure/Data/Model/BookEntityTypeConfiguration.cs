using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.Isbn13).HasName("PK__Books__3BF79E0302F9419A");

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .IsUnicode(false)
            .HasColumnName("ISBN13");
        builder.Property(e => e.Language).HasMaxLength(100);
        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.Title).HasMaxLength(100);

        builder.HasOne(d => d.Format).WithMany(p => p.Books)
            .HasForeignKey(d => d.FormatId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Books__FormatId__38A457AD");

        builder.HasOne(d => d.Genre).WithMany(p => p.Books)
            .HasForeignKey(d => d.GenreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Books__GenreId__37B03374");

        builder.HasMany(d => d.Authors).WithMany(p => p.Isbn13s)
            .UsingEntity<Dictionary<string, object>>(
                "BookAuthor",
                r => r.HasOne<Author>().WithMany()
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookAutho__Autho__3C74E891"),
                l => l.HasOne<Book>().WithMany()
                    .HasForeignKey("Isbn13")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookAutho__ISBN1__3B80C458"),
                j =>
                {
                    j.HasKey("Isbn13", "AuthorId").HasName("PK__BookAuth__6CFA31C0CC300566");
                    j.ToTable("BookAuthors");
                    j.IndexerProperty<string>("Isbn13")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnName("ISBN13");
                });
    }
}
