using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MicrosoftTranslatorProvider.Helpers;

public class PasswordHelper
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static RoutedEventHandler _003C0_003E__PasswordChanged;
	}

	public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), (PropertyMetadata)new FrameworkPropertyMetadata((object)string.Empty, new PropertyChangedCallback(OnPasswordPropertyChanged)));

	public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata((object)false, new PropertyChangedCallback(Attach)));

	private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

	public static void SetAttach(DependencyObject dp, bool value)
	{
		dp.SetValue(AttachProperty, (object)value);
	}

	public static bool GetAttach(DependencyObject dp)
	{
		return (bool)dp.GetValue(AttachProperty);
	}

	public static void SetPassword(DependencyObject dp, string value)
	{
		dp.SetValue(PasswordProperty, (object)value);
	}

	public static string GetPassword(DependencyObject dp)
	{
		return (string)dp.GetValue(PasswordProperty);
	}

	private static void SetIsUpdating(DependencyObject dp, bool value)
	{
		dp.SetValue(IsUpdatingProperty, (object)value);
	}

	private static bool GetIsUpdating(DependencyObject dp)
	{
		return (bool)dp.GetValue(IsUpdatingProperty);
	}

	private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		PasswordBox val = (PasswordBox)(object)((sender is PasswordBox) ? sender : null);
		object obj = _003C_003EO._003C0_003E__PasswordChanged;
		if (obj == null)
		{
			RoutedEventHandler val2 = PasswordChanged;
			_003C_003EO._003C0_003E__PasswordChanged = val2;
			obj = (object)val2;
		}
		val.PasswordChanged -= (RoutedEventHandler)obj;
		if (!GetIsUpdating((DependencyObject)(object)val))
		{
			val.Password = (string)((DependencyPropertyChangedEventArgs)(ref e)).NewValue;
		}
		object obj2 = _003C_003EO._003C0_003E__PasswordChanged;
		if (obj2 == null)
		{
			RoutedEventHandler val3 = PasswordChanged;
			_003C_003EO._003C0_003E__PasswordChanged = val3;
			obj2 = (object)val3;
		}
		val.PasswordChanged += (RoutedEventHandler)obj2;
	}

	private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		PasswordBox val = (PasswordBox)(object)((sender is PasswordBox) ? sender : null);
		if (val == null)
		{
			return;
		}
		if ((bool)((DependencyPropertyChangedEventArgs)(ref e)).OldValue)
		{
			object obj = _003C_003EO._003C0_003E__PasswordChanged;
			if (obj == null)
			{
				RoutedEventHandler val2 = PasswordChanged;
				_003C_003EO._003C0_003E__PasswordChanged = val2;
				obj = (object)val2;
			}
			val.PasswordChanged -= (RoutedEventHandler)obj;
		}
		if ((bool)((DependencyPropertyChangedEventArgs)(ref e)).NewValue)
		{
			object obj2 = _003C_003EO._003C0_003E__PasswordChanged;
			if (obj2 == null)
			{
				RoutedEventHandler val3 = PasswordChanged;
				_003C_003EO._003C0_003E__PasswordChanged = val3;
				obj2 = (object)val3;
			}
			val.PasswordChanged += (RoutedEventHandler)obj2;
		}
	}

	private static void PasswordChanged(object sender, RoutedEventArgs e)
	{
		PasswordBox val = (PasswordBox)((sender is PasswordBox) ? sender : null);
		SetIsUpdating((DependencyObject)(object)val, value: true);
		SetPassword((DependencyObject)(object)val, (val != null) ? val.Password : null);
		SetIsUpdating((DependencyObject)(object)val, value: false);
	}
}
