using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MicrosoftTranslatorProvider.UiHelpers;

public class TextBoxWatermarkHelper : DependencyObject
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static RoutedEventHandler _003C0_003E__TextChanged;

		public static RoutedEventHandler _003C1_003E__ButtonClicked;

		public static RoutedEventHandler _003C2_003E__OnControlLostFocus;

		public static RoutedEventHandler _003C3_003E__OnControlGotFocus;
	}

	public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.RegisterAttached("ButtonCommandParameter", typeof(object), typeof(TextBoxWatermarkHelper), (PropertyMetadata)new FrameworkPropertyMetadata((PropertyChangedCallback)null));

	public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.RegisterAttached("ButtonCommand", typeof(ICommand), typeof(TextBoxWatermarkHelper), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, new PropertyChangedCallback(ButtonCommandOrClearTextChanged)));

	public static readonly DependencyProperty IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsClearTextButtonBehaviorEnabled", typeof(bool), typeof(TextBoxWatermarkHelper), (PropertyMetadata)new FrameworkPropertyMetadata((object)false, new PropertyChangedCallback(IsClearTextButtonBehaviorEnabledChanged)));

	public static readonly DependencyProperty IsWatermarkVisibleProperty = DependencyProperty.RegisterAttached("IsWatermarkVisible", typeof(bool), typeof(TextBoxWatermarkHelper));

	public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(TextBoxWatermarkHelper), new PropertyMetadata((object)"Watermark", new PropertyChangedCallback(OnWatermarkTextChanged)));

	public static void ButtonClicked(object sender, RoutedEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Button child = (Button)sender;
		DependencyObject val = GetAncestors((DependencyObject)(object)child).FirstOrDefault((Func<DependencyObject, bool>)((DependencyObject a) => a is TextBox || a is PasswordBox || a is ComboBox));
		ICommand buttonCommand = GetButtonCommand(val);
		object parameter = GetButtonCommandParameter(val) ?? val;
		if (buttonCommand != null && buttonCommand.CanExecute(parameter))
		{
			buttonCommand.Execute(parameter);
		}
	}

	public static ICommand GetButtonCommand(DependencyObject d)
	{
		return (ICommand)d.GetValue(ButtonCommandProperty);
	}

	public static object GetButtonCommandParameter(DependencyObject d)
	{
		return d.GetValue(ButtonCommandParameterProperty);
	}

	public static bool GetIsWatermarkVisible(DependencyObject d)
	{
		return (bool)d.GetValue(IsWatermarkVisibleProperty);
	}

	public static string GetWatermarkText(DependencyObject d)
	{
		return (string)d.GetValue(WatermarkTextProperty);
	}

	public static void SetButtonCommand(DependencyObject d, object value)
	{
		d.SetValue(ButtonCommandProperty, value);
	}

	public static void SetButtonCommandParameter(DependencyObject d, object value)
	{
		d.SetValue(ButtonCommandParameterProperty, value);
	}

	public static void SetIsClearTextButtonBehaviorEnabled(Button button, bool value)
	{
		((DependencyObject)button).SetValue(IsClearTextButtonBehaviorEnabledProperty, (object)value);
	}

	public static void SetWatermarkText(DependencyObject d, string text)
	{
		d.SetValue(WatermarkTextProperty, (object)text);
	}

	private static void ButtonCommandOrClearTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		TextBox val = (TextBox)(object)((d is TextBox) ? d : null);
		if (val != null)
		{
			object obj = _003C_003EO._003C0_003E__TextChanged;
			if (obj == null)
			{
				RoutedEventHandler val2 = TextChanged;
				_003C_003EO._003C0_003E__TextChanged = val2;
				obj = (object)val2;
			}
			((FrameworkElement)val).Loaded -= (RoutedEventHandler)obj;
			object obj2 = _003C_003EO._003C0_003E__TextChanged;
			if (obj2 == null)
			{
				RoutedEventHandler val3 = TextChanged;
				_003C_003EO._003C0_003E__TextChanged = val3;
				obj2 = (object)val3;
			}
			((FrameworkElement)val).Loaded += (RoutedEventHandler)obj2;
			if (((FrameworkElement)val).IsLoaded)
			{
				TextChanged(val, new RoutedEventArgs());
			}
		}
	}

	private static void IsClearTextButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		if (((DependencyPropertyChangedEventArgs)(ref e)).OldValue == ((DependencyPropertyChangedEventArgs)(ref e)).NewValue)
		{
			return;
		}
		Button val = (Button)(object)((d is Button) ? d : null);
		if (val == null)
		{
			return;
		}
		object obj = _003C_003EO._003C1_003E__ButtonClicked;
		if (obj == null)
		{
			RoutedEventHandler val2 = ButtonClicked;
			_003C_003EO._003C1_003E__ButtonClicked = val2;
			obj = (object)val2;
		}
		((ButtonBase)val).Click -= (RoutedEventHandler)obj;
		if ((bool)((DependencyPropertyChangedEventArgs)(ref e)).NewValue)
		{
			object obj2 = _003C_003EO._003C1_003E__ButtonClicked;
			if (obj2 == null)
			{
				RoutedEventHandler val3 = ButtonClicked;
				_003C_003EO._003C1_003E__ButtonClicked = val3;
				obj2 = (object)val3;
			}
			((ButtonBase)val).Click += (RoutedEventHandler)obj2;
		}
	}

	private static void OnControlGotFocus(object sender, RoutedEventArgs e)
	{
		object obj = ((sender is TextBox) ? sender : null);
		if (obj != null)
		{
			((DependencyObject)obj).SetValue(IsWatermarkVisibleProperty, (object)false);
		}
	}

	private static void OnControlLostFocus(object sender, RoutedEventArgs e)
	{
		TextBox val = (TextBox)((sender is TextBox) ? sender : null);
		if (val != null && string.IsNullOrEmpty(val.Text))
		{
			((DependencyObject)val).SetValue(IsWatermarkVisibleProperty, (object)true);
		}
	}

	private static void OnWatermarkTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		if (d != null)
		{
			d.SetValue(IsWatermarkVisibleProperty, (object)true);
		}
		TextBox val = (TextBox)(object)((d is TextBox) ? d : null);
		if (val != null)
		{
			object obj = _003C_003EO._003C2_003E__OnControlLostFocus;
			if (obj == null)
			{
				RoutedEventHandler val2 = OnControlLostFocus;
				_003C_003EO._003C2_003E__OnControlLostFocus = val2;
				obj = (object)val2;
			}
			((UIElement)val).LostFocus += (RoutedEventHandler)obj;
			object obj2 = _003C_003EO._003C3_003E__OnControlGotFocus;
			if (obj2 == null)
			{
				RoutedEventHandler val3 = OnControlGotFocus;
				_003C_003EO._003C3_003E__OnControlGotFocus = val3;
				obj2 = (object)val3;
			}
			((UIElement)val).GotFocus += (RoutedEventHandler)obj2;
		}
	}

	private static void SetTextLength<TDependencyObject>(TDependencyObject sender, Func<TDependencyObject, int> _) where TDependencyObject : DependencyObject
	{
		if (sender != null)
		{
			object obj = sender;
			TextBox val = (TextBox)((obj is TextBox) ? obj : null);
			if (val != null && !string.IsNullOrEmpty(val.Text))
			{
				((DependencyObject)val).SetValue(IsWatermarkVisibleProperty, (object)false);
			}
		}
	}

	private static void TextChanged(object sender, RoutedEventArgs e)
	{
		SetTextLength<TextBox>((TextBox)((sender is TextBox) ? sender : null), (Func<TextBox, int>)((TextBox textBox) => textBox.Text.Length));
	}

	private static IEnumerable<DependencyObject> GetAncestors(DependencyObject child)
	{
		for (DependencyObject parent = VisualTreeHelper.GetParent(child); parent != null; parent = VisualTreeHelper.GetParent(parent))
		{
			yield return parent;
		}
	}
}
