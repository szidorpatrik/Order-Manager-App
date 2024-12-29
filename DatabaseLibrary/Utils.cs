
using Languages;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace DatabaseLibrary {
	public static class Utils {
		public static string GetFormattedPrice(IStringLocalizer<OrderManagerAppLanguages> Localizer, double? totalPrice) {
			if (!totalPrice.HasValue || totalPrice <= 0)
				return Localizer["N/A"];

			var currentCulture = CultureInfo.CurrentCulture;

			return currentCulture.Name switch {
				"hu-HU" => totalPrice.Value.ToString("C0", currentCulture),
				_ => totalPrice.Value.ToString("C", currentCulture)
			};
		}
	}
}
