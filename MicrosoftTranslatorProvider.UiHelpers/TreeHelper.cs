using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MicrosoftTranslatorProvider.UiHelpers;

public static class TreeHelper
{
	public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject child)
	{
		for (DependencyObject parent = VisualTreeHelper.GetParent(child); parent != null; parent = VisualTreeHelper.GetParent(parent))
		{
			yield return parent;
		}
	}
}
