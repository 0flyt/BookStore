using System;
using System.Collections.Generic;

namespace BookStore.Domain;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public DateOnly HireDate { get; set; }

    public int StoreId { get; set; }

    public virtual ICollection<Order> OrderOrderingEmployees { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderReceivedEmployees { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSenderEmployees { get; set; } = new List<Order>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual Store Store { get; set; } = null!;
}
