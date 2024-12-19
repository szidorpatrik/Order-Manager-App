namespace DatabaseLibrary.Models;

using System;

public class OrderItem
{
    public int Id { get; set; }
    public Item Item { get; set; }
    public int Quantity { get; set; }
    
    public override String ToString() => Item.Name;
}