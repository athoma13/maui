using Android.Util;
using Android.Webkit;

namespace maui;

internal class CustomWebChromeClient : WebChromeClient
{
    public override async void OnPermissionRequest(PermissionRequest? request)
    {
        Log.Info("DBG", "Permission Requested - " + request);
        var resources = request?.GetResources() ?? [];
        if (resources.Any(r => r == PermissionRequest.ResourceVideoCapture))
        {
            var p = await Permissions.RequestAsync<Permissions.Camera>();
            if (p == PermissionStatus.Granted)
            {
                Log.Info("DBG", "Permission Granted");
                request?.Grant([PermissionRequest.ResourceVideoCapture]);
            }
            else
            {
                Log.Info("DBG", "Permission Denied");
                request?.Deny();
            }
        }
    }

    public override void OnGeolocationPermissionsHidePrompt()
    {
        Log.Info("DBG", "OnGeolocationPermissionsHidePrompt");
        base.OnGeolocationPermissionsHidePrompt();
    }

    public override void OnGeolocationPermissionsShowPrompt(string? origin, GeolocationPermissions.ICallback? callback)
    {
        Log.Info("DBG", "OnGeolocationPermissionsShowPrompt");
        Permissions.RequestAsync<Permissions.LocationWhenInUse>().ContinueWith(t => {
            Log.Info("DBG", "OnGeolocationPermissionsShowPrompt - Result:" + t.Result);
            callback?.Invoke(origin, t.Result == PermissionStatus.Granted, false);
        });
    }
}