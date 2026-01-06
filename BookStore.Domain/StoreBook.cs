using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class StoreBook
{
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int QuantityInStock { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
