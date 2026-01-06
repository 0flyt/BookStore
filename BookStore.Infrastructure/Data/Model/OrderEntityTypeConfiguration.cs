using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF1815DF46");

        builder.Property(e => e.OrderDate).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.OrderType)
            .HasMaxLength(100)
            .HasDefaultValue("FrånUtgivare");
        builder.Property(e => e.OrderingEmployeeId).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.ReceivedDate).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.ReceivedEmployeeId).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.SenderEmployeeId).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.SenderStoreId).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.ShippedDate).HasDefaultValueSql("(NULL)");
        builder.Property(e => e.StatusId).HasDefaultValue(1);

        builder.HasOne(d => d.DestinationStore).WithMany(p => p.OrderDestinationStores)
            .HasForeignKey(d => d.DestinationStoreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Orders__Destinat__5CE1B823");

        builder.HasOne(d => d.OrderingEmployee).WithMany(p => p.OrderOrderingEmployees)
            .HasForeignKey(d => d.OrderingEmployeeId)
            .HasConstraintName("FK__Orders__Ordering__5ECA0095");

        builder.HasOne(d => d.ReceivedEmployee).WithMany(p => p.OrderReceivedEmployees)
            .HasForeignKey(d => d.ReceivedEmployeeId)
            .HasConstraintName("FK__Orders__Received__60B24907");

        builder.HasOne(d => d.SenderEmployee).WithMany(p => p.OrderSenderEmployees)
            .HasForeignKey(d => d.SenderEmployeeId)
            .HasConstraintName("FK__Orders__SenderEm__5FBE24CE");

        builder.HasOne(d => d.SenderStore).WithMany(p => p.OrderSenderStores)
            .HasForeignKey(d => d.SenderStoreId)
            .HasConstraintName("FK__Orders__SenderSt__5DD5DC5C");

        builder.HasOne(d => d.Status).WithMany(p => p.Orders)
            .HasForeignKey(d => d.StatusId)
            .HasConstraintName("FK__Orders__StatusId__61A66D40");
    }
}
