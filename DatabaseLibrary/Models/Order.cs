using System;
using System.Collections.Generic;

namespace DatabaseLibrary.Models;

public class Order {
	public DateTime        DateCreated { get; set; }
	public DateTime        DateEnd     { get; set; }
	public DateTime        DateStart   { get; set; }
	public int             Id          { get; set; }
	public List<OrderItem> OrderItems  { get; set; } = [];
	public int             OrderNumber { get; set; }

	public override string ToString() => $"#{OrderNumber}";
}
