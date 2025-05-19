using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MicrosoftTranslatorProvider.Converters;

public class EmptyToVisibility : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (object)(Visibility)(string.IsNullOrEmpty(value?.ToString()) ? 2 : 0);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
