using System;
using System.Globalization;
using System.Windows.Data;

namespace MicrosoftTranslatorProvider.Converters;

public class InvertableStringEmptyToBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = string.IsNullOrEmpty(value as string);
		if (parameter == null)
		{
			return flag;
		}
		return ((Parameters)Enum.Parse(typeof(Parameters), (string)parameter) == Parameters.Inverted) ? (!flag) : flag;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
