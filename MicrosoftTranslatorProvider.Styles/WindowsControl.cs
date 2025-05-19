using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Sdl.Desktop.Platform.Controls.Controls;

namespace MicrosoftTranslatorProvider.Styles;

public class WindowsControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty HelpProperty = DependencyProperty.Register("ControlHelp", typeof(string), typeof(WindowsControl), new PropertyMetadata((object)string.Empty));

	public static readonly DependencyProperty CloseProperty = DependencyProperty.Register("ControlClose", typeof(string), typeof(WindowsControl), new PropertyMetadata((object)string.Empty));

	public static readonly DependencyProperty MaximizeProperty = DependencyProperty.Register("ControlMaximize", typeof(string), typeof(WindowsControl), new PropertyMetadata((object)string.Empty));

	public static readonly DependencyProperty MinimizeProperty = DependencyProperty.Register("ControlMinimize", typeof(string), typeof(WindowsControl), new PropertyMetadata((object)string.Empty));

	public static readonly DependencyProperty RestoreProperty = DependencyProperty.Register("ControlRestore", typeof(string), typeof(WindowsControl), new PropertyMetadata((object)string.Empty));

	internal Grid WindowTitleBar;

	internal Button CloseButton;

	private bool _contentLoaded;

	public string ControlHelp
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(HelpProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(HelpProperty, (object)value);
		}
	}

	public string ControlClose
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(CloseProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(CloseProperty, (object)value);
		}
	}

	public string ControlMaximize
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(MaximizeProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(MaximizeProperty, (object)value);
		}
	}

	public string ControlMinimize
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(MinimizeProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(MinimizeProperty, (object)value);
		}
	}

	public string ControlRestore
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(RestoreProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(RestoreProperty, (object)value);
		}
	}

	public WindowsControl()
	{
		InitializeComponent();
		((DispatcherObject)this).Dispatcher.BeginInvoke((DispatcherPriority)6, (Delegate)(Action)delegate
		{
			if (string.IsNullOrWhiteSpace(ControlClose))
			{
				ControlClose = PluginResources.WindowsControl_Close;
			}
		});
	}

	private void CloseButton_OnClick(object sender, RoutedEventArgs e)
	{
		WindowsControlUtils.ForWindowFromFrameworkElement(sender, (Action<Window>)delegate(Window w)
		{
			w.Close();
		});
	}

	private void MinButton_Click(object sender, RoutedEventArgs e)
	{
		WindowsControlUtils.ForWindowFromFrameworkElement(sender, (Action<Window>)delegate(Window w)
		{
			w.WindowState = (WindowState)1;
		});
	}

	private void MaxButton_Click(object sender, RoutedEventArgs e)
	{
		WindowsControlUtils.ForWindowFromFrameworkElement(sender, (Action<Window>)delegate(Window w)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			w.WindowState = (WindowState)(((int)w.WindowState != 2) ? 2 : 0);
		});
	}

	private void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (e.ClickCount > 1)
		{
			MaxButton_Click(sender, (RoutedEventArgs)(object)e);
		}
		else if ((int)((MouseEventArgs)e).LeftButton == 1)
		{
			WindowsControlUtils.ForWindowFromFrameworkElement(sender, (Action<Window>)delegate(Window w)
			{
				w.DragMove();
			});
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/MicrosoftTranslatorProvider;component/styles/windowscontrol.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			WindowTitleBar = (Grid)target;
			((UIElement)WindowTitleBar).MouseLeftButtonDown += new MouseButtonEventHandler(TitleBarMouseLeftButtonDown);
			break;
		case 2:
			CloseButton = (Button)target;
			((ButtonBase)CloseButton).Click += new RoutedEventHandler(CloseButton_OnClick);
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
