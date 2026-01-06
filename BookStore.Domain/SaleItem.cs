using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class SaleItem
{
    public int SaleId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal SalePrice { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
