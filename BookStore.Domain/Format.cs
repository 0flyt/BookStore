using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class Format
{
    public int FormatId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
