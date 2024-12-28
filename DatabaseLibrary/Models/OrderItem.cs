namespace DatabaseLibrary.Models;

public class OrderItem {
	public int Id { get; set; }
	public Item Item { get; set; }
	public int Quantity { get; set; }

	public override string ToString() {
		if (Item == null) {
			return base.ToString();
		}

		return $"{Item.Name} ({Quantity})";
	}
}
