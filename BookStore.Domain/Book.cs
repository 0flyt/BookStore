using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int GenreId { get; set; }

    public int FormatId { get; set; }

    public virtual Format Format { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual ICollection<StoreBook> StoreBooks { get; set; } = new List<StoreBook>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
