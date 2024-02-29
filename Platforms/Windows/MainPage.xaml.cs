
using Microsoft.Web.WebView2.Core;

namespace maui;
public partial class MainPage
{

    const string FILE = @"C:\Users\Arnaud\Documents\log.txt";

    private static void Log(string message)
    {
        File.AppendAllText(FILE, DateTime.Now + "   " + message + Environment.NewLine);
    }

    partial void InitializeWebView()
    {
        var platformView = (wv.Handler as Microsoft.Maui.Handlers.IWebViewHandler)?.PlatformView;
        if (platformView == null) { return; }

        platformView.CoreWebView2Initialized += async (s, e) =>
        {
            var coreWebView2 = platformView.CoreWebView2;
            if (coreWebView2 == null) { return; }

            // Clear out all cached permissions 
            var p = await coreWebView2.Profile.GetNonDefaultPermissionSettingsAsync();
            p.ToList().ForEach(x => Log($"{x.PermissionKind}  {x.PermissionOrigin} {x.PermissionState}"));
            p.ToList().ForEach(async x => await coreWebView2.Profile.SetPermissionStateAsync(x.PermissionKind, x.PermissionOrigin, CoreWebView2PermissionState.Default));

            coreWebView2.PermissionRequested += (s, e) =>
            {
                Log("PermissionRequested: " + e.PermissionKind);
                Log("Origin" + e.Uri);

                var permissionRequest = e.PermissionKind == CoreWebView2PermissionKind.Geolocation ? Permissions.RequestAsync<Permissions.LocationWhenInUse>() :
                                    e.PermissionKind == CoreWebView2PermissionKind.Camera ? Permissions.RequestAsync<Permissions.Camera>() : null;
                if (permissionRequest == null) { return; }

                var def = e.GetDeferral();
                permissionRequest?.ContinueWith((t) =>
                    {
                        // IMPORTANT: When granting/denying permissions, if the result is saved in the current profile, then the permission won't be requested again.
                        // To avoid this, set SavesInProfile = false, so the permission will be requested again next time.
                        
                        e.SavesInProfile = false;
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