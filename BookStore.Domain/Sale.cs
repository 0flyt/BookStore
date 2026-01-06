using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class Sale
{
    public int SaleId { get; set; }

    public int StoreId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? SaleDateTime { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual Store Store { get; set; } = null!;
}
