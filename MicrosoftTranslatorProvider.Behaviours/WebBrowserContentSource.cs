using System.Windows;
using System.Windows.Controls;

namespace MicrosoftTranslatorProvider.Behaviours;

public static class WebBrowserContentSource
{
	public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(WebBrowserContentSource), (PropertyMetadata)new FrameworkPropertyMetadata(new PropertyChangedCallback(OnHtmlChanged)));

	[AttachedPropertyBrowsableForType(typeof(WebBrowser))]
	public static string GetHtml(WebBrowser webBrowser)
	{
		return (string)((DependencyObject)webBrowser).GetValue(HtmlProperty);
	}

	public static void SetHtml(WebBrowser webBrowser, string value)
	{
		((DependencyObject)webBrowser).SetValue(HtmlProperty, (object)value);
	}

	private static void OnHtmlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		string text = ((DependencyPropertyChangedEventArgs)(ref e)).NewValue as string;
		WebBrowser val = (WebBrowser)(object)((obj is WebBrowser) ? obj : null);
		if (val != null && !string.IsNullOrEmpty(text))
		{
			val.NavigateToString(text);
		}
	}
}
