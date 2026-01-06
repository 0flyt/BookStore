using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class MostSalesGenre
{
    public string Genre { get; set; } = null!;

    public int? OfBooks { get; set; }

    public int? OfSales { get; set; }
}
