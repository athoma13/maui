
using Microsoft.Web.WebView2.Core;

namespace maui;
public partial class MainPage
{
    partial void InitializeWebView()
    {
        var platformView = (wv.Handler as Microsoft.Maui.Handlers.IWebViewHandler)?.PlatformView;
        if (platformView == null) { return; }

        platformView.CoreWebView2Initialized += (s, e) =>
        {
            var coreWebView2 = platformView.CoreWebView2;
            if (coreWebView2 == null) { return; }
            coreWebView2.PermissionRequested += (s, e) =>
            {
                var permissionRequest = e.PermissionKind == CoreWebView2PermissionKind.Geolocation ? Permissions.RequestAsync<Permissions.LocationWhenInUse>() :
                                    e.PermissionKind == CoreWebView2PermissionKind.Camera ? Permissions.RequestAsync<Permissions.Camera>() : null;
                if (permissionRequest == null) { return; }

                var def = e.GetDeferral();
                permissionRequest?.ContinueWith((t) =>
                    {
                        if (t.Result == PermissionStatus.Granted)
                        {
                            e.State = CoreWebView2PermissionState.Allow;
                        }
                        else
                        {
                            e.State = CoreWebView2PermissionState.Deny;
                        }
                        def?.Complete();
                    });
            };
        };
    }
}