using CommunityToolkit.Maui;
using DatabaseLibrary;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;

namespace OrderManagerApp;

public static class MauiProgram {
	public static MauiApp CreateMauiApp() {
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts => {
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddLocalization();
		builder.Services.AddSingleton<LiteDbService>();

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddMudServices(config => {
			config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomEnd;

			config.SnackbarConfiguration.PreventDuplicates = true;
			config.SnackbarConfiguration.ShowCloseIcon = true;
			config.SnackbarConfiguration.VisibleStateDuration = 2500;
			config.SnackbarConfiguration.HideTransitionDuration = 250;
			config.SnackbarConfiguration.ShowTransitionDuration = 250;
			config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
		});


#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
