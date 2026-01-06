using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class Order
{
    public int OrderId { get; set; }

    public int DestinationStoreId { get; set; }

    public int? OrderingEmployeeId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? SenderStoreId { get; set; }

    public int? SenderEmployeeId { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int? ReceivedEmployeeId { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public int? StatusId { get; set; }

    public string OrderType { get; set; } = null!;

    public virtual Store DestinationStore { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Employee? OrderingEmployee { get; set; }

    public virtual Employee? ReceivedEmployee { get; set; }

    public virtual Employee? SenderEmployee { get; set; }

    public virtual Store? SenderStore { get; set; }

    public virtual OrderStatus? Status { get; set; }
}
