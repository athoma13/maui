using Microsoft.Maui.Handlers;

namespace maui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		Loaded += MainPage_Loaded;
	}

	private void MainPage_Loaded(object sender, EventArgs e)
	{
		var platformView = (wv.Handler as IWebViewHandler)?.PlatformView;

		if (platformView != null)
		{
			platformView.CoreWebView2Initialized += (s, e) =>
			{
				var coreWebView2 = platformView.CoreWebView2;
				if (coreWebView2 != null)
				{
					coreWebView2.PermissionRequested += (s, e) =>
					{
						// Allow all permissions request by the WebView. (of course, we need the app to have been granted those permissions first.)
						e.State = Microsoft.Web.WebView2.Core.CoreWebView2PermissionState.Allow;
					};
				}
			};
		}
	}
}

