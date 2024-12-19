namespace DatabaseLibrary.Models;

public class Item {
	public int    Id    { get; set; }
	public string Name  { get; set; }
	public double Price { get; set; }

	public override string ToString() => Name;
}
