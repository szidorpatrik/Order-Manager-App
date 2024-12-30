using System;

namespace DatabaseLibrary.Models;

public class Order {
	public DateTime    DateCreated { get; set; }
	public DateTime    DateEnd     { get; set; }
	public DateTime    DateStart   { get; set; }
	public int         Id          { get; set; }
	public OrderItem[] OrderItems  { get; set; } = [];
	public int?        OrderNumber { get; set; } = 0;

	public override string ToString() => $"#{OrderNumber}";
}
