namespace DatabaseLibrary.Models;

public class Item {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public double Price { get; set; }

	public override bool Equals(object? obj) {
		if (obj is not Item other) return false;
		return Id == other.Id;
	}

	public override int GetHashCode() => Id.GetHashCode();
}
