using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class EmployeeSalesAndOrder
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string Store { get; set; } = null!;

    public int? OfSales { get; set; }

    public int OfSoldArticles { get; set; }

    public decimal TotalSalePriceInSwedishKr { get; set; }

    public int? OfOrders { get; set; }

    public int? OfOrdersSend { get; set; }

    public int? OfOrdersRecieved { get; set; }
}
