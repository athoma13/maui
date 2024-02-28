using Android.Util;

namespace maui;
public partial class MainPage
{
    partial void InitializeWebView()
    {
        var vw = (wv.Handler as Microsoft.Maui.Handlers.IWebViewHandler)?.PlatformView;
        Log.Info("DBG", "MainPage XX" + $"vw: {vw != null}");
        if (vw != null && vw.Settings != null)
        {
            vw.Settings.SetGeolocationEnabled(true);
            vw.SetWebChromeClient(new CustomWebChromeClient());
            Log.Info("DBG", $"MainPage - Clkicene Set vw: {vw != null}");
        }
    }
}