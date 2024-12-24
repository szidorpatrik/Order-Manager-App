using System.Globalization;

namespace DatabaseLibrary.Models;

public class Item {
	public int Id { get; set; }
	public string Name { get; set; }
	public double Price { get; set; }

	public override string ToString() => Name;

	public string GetFormattedPrice() {
		var currentCulture = CultureInfo.CurrentCulture;

		return currentCulture.Name switch {
			"hu-HU" => Price.ToString("C0", currentCulture),
			_ => Price.ToString("C", currentCulture)
		};
	}
}
