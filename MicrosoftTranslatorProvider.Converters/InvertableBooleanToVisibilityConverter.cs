using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MicrosoftTranslatorProvider.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class InvertableBooleanToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = value != null && (bool)value;
		if (parameter == null)
		{
			return (object)(Visibility)((!flag) ? 2 : 0);
		}
		Parameters parameters = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);
		if (1 == 0)
		{
		}
		object result = ((parameters != 0) ? ((object)(Visibility)((!flag) ? 2 : 0)) : ((object)(Visibility)(flag ? 2 : 0)));
		if (1 == 0)
		{
		}
		return result;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
