using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MicrosoftTranslatorProvider.Behaviours;

public static class TextBoxCommandBehavior
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static RoutedEventHandler _003C0_003E__TextBox_GotFocus;
	}

	public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(TextBoxCommandBehavior), new PropertyMetadata((object)null, new PropertyChangedCallback(OnCommandChanged)));

	public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(TextBoxCommandBehavior), new PropertyMetadata((PropertyChangedCallback)null));

	public static ICommand GetCommand(DependencyObject obj)
	{
		return (ICommand)obj.GetValue(CommandProperty);
	}

	public static void SetCommand(DependencyObject obj, ICommand value)
	{
		obj.SetValue(CommandProperty, (object)value);
	}

	public static object GetCommandParameter(DependencyObject obj)
	{
		return obj.GetValue(CommandParameterProperty);
	}

	public static void SetCommandParameter(DependencyObject obj, object value)
	{
		obj.SetValue(CommandParameterProperty, value);
	}

	private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		TextBox val = (TextBox)(object)((d is TextBox) ? d : null);
		if (val == null)
		{
			return;
		}
		object obj = _003C_003EO._003C0_003E__TextBox_GotFocus;
		if (obj == null)
		{
			RoutedEventHandler val2 = TextBox_GotFocus;
			_003C_003EO._003C0_003E__TextBox_GotFocus = val2;
			obj = (object)val2;
		}
		((UIElement)val).GotFocus -= (RoutedEventHandler)obj;
		if (((DependencyPropertyChangedEventArgs)(ref e)).NewValue is ICommand)
		{
			object obj2 = _003C_003EO._003C0_003E__TextBox_GotFocus;
			if (obj2 == null)
			{
				RoutedEventHandler val3 = TextBox_GotFocus;
				_003C_003EO._003C0_003E__TextBox_GotFocus = val3;
				obj2 = (object)val3;
			}
			((UIElement)val).GotFocus += (RoutedEventHandler)obj2;
		}
	}

	private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
	{
		TextBox val = (TextBox)((sender is TextBox) ? sender : null);
		if (val != null)
		{
			ICommand command = GetCommand((DependencyObject)(object)val);
			object commandParameter = GetCommandParameter((DependencyObject)(object)val);
			if ((command?.CanExecute(commandParameter)).Value)
			{
				command.Execute(commandParameter);
			}
		}
	}
}
