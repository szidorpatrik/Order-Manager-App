namespace DatabaseLibrary.Models;

using System;
using System.Collections.Generic;

public class Order
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public DateTime DateCreated { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }

    public override String ToString() => $"#{OrderNumber}";
}
