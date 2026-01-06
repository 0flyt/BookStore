using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class AuthorStatistic
{
    public string Name { get; set; } = null!;

    public string Age { get; set; } = null!;

    public string Titles { get; set; } = null!;

    public string TotalInventoryValue { get; set; } = null!;
}
