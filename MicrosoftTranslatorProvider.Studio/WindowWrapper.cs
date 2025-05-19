using System;
using System.Windows.Forms;

namespace MicrosoftTranslatorProvider.Studio;

public class WindowWrapper : IWin32Window
{
	public IntPtr Handle { get; }

	public WindowWrapper(IntPtr handle)
	{
		Handle = handle;
	}
}
