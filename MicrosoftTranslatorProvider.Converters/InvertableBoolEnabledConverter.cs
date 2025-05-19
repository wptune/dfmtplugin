using System;
using System.Globalization;
using System.Windows.Data;

namespace MicrosoftTranslatorProvider.Converters;

public class InvertableBoolEnabledConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = value != null && (bool)value;
		if (parameter == null)
		{
			return flag;
		}
		return ((Parameters)Enum.Parse(typeof(Parameters), (string)parameter) == Parameters.Inverted) ? ((object)(!flag)) : ((object)flag);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
