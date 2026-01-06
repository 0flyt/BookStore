using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookStore.Infrastructure.Data.Model;

public partial class BookStoreContext : DbContext
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorStatistic> AuthorStatistics { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeSalesAndOrder> EmployeeSalesAndOrders { get; set; }

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<MostSalesGenre> MostSalesGenres { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreBook> StoreBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<BookStoreContext>().Build();
        var connectionString = config["ConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new StoreBookEntityTypeConfiguration().Configure(modelBuilder.Entity<StoreBook>());
        new StoreEntityTypeConfiguration().Configure(modelBuilder.Entity<Store>());
        new SaleItemEntityTypeConfiguration().Configure(modelBuilder.Entity<SaleItem>());
        new SaleEntityTypeConfiguration().Configure(modelBuilder.Entity<Sale>());
        new OrderStatusEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderStatus>());
        new OrderDetailEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderDetail>());
        new OrderEntityTypeConfiguration().Configure(modelBuilder.Entity<Order>());
        new MostSalesGenreEntityTypeConfiguration().Configure(modelBuilder.Entity<MostSalesGenre>());
        new GenreEntityTypeConfiguration().Configure(modelBuilder.Entity<Genre>());
        new FormatEntityTypeConfiguration().Configure(modelBuilder.Entity<Format>());
        new EmployeeSalesAndOrderEntityTypeConfiguration().Configure(modelBuilder.Entity<EmployeeSalesAndOrder>());
        new EmployeeEntityTypeConfiguration().Configure(modelBuilder.Entity<Employee>());
        new BookEntityTypeConfiguration().Configure(modelBuilder.Entity<Book>());
        new AuthorStatisticEntityTypeConfiguration().Configure(modelBuilder.Entity<AuthorStatistic>());
        new AuthorEntityTypeConfiguration().Configure(modelBuilder.Entity<Author>());
        new StoreBookEntityTypeConfiguration().Configure(modelBuilder.Entity<StoreBook>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}