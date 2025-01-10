using System.Globalization;
using Languages;
using Microsoft.Extensions.Localization;

namespace DatabaseLibrary;

public static class Utils {
	public static string GetFormattedPrice(IStringLocalizer<OrderManagerAppLanguages> localizer, double? totalPrice) {
		if (totalPrice is null or <= 0)
			return localizer["N/A"];

		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		return currentCulture.Name switch {
			"hu-HU" => totalPrice.Value.ToString("C0", currentCulture),
			_       => totalPrice.Value.ToString("C", currentCulture)
		};
	}
}
