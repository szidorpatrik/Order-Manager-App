using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Application=Microsoft.Maui.Controls.Application;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace OrderManagerApp;

using System.Globalization;

public partial class App : Application {
	public App() {
		InitializeComponent();

#if WINDOWS
        int WindowWidth = 800;
        int WindowHeight = 1200;

        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            nint windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
        });
#endif

		ApplyLocalization();
		ApplyTheme();

		MainPage = new MainPage();
		Current?.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
	}

	private void ApplyLocalization() {
		string language = Preferences.Get("Language", "en-US");
		CultureInfo culture = new(language);

		Thread.CurrentThread.CurrentCulture = culture;
		Thread.CurrentThread.CurrentUICulture = culture;
		CultureInfo.DefaultThreadCurrentCulture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;
	}

	private void ApplyTheme() {
		Current!.UserAppTheme = Enum.Parse<AppTheme>(Preferences.Get("AppTheme", "Unspecified"));
	}

	public static void SetAppTheme(AppTheme theme)
	{
		Current!.UserAppTheme = theme;
		Preferences.Set("AppTheme", theme.ToString());
	}
}
