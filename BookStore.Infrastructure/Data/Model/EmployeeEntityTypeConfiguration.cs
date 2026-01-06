using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Data.Model;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F113B8D72B1");

        builder.Property(e => e.FirstName).HasMaxLength(100);
        builder.Property(e => e.LastName).HasMaxLength(100);

        builder.HasOne(d => d.Store).WithMany(p => p.Employees)
            .HasForeignKey(d => d.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Employees__Store__41399DAE");
    }
}
