using System;
using System.Windows.Forms;

namespace CustomMTTranslationProvider;

internal class MtWindowWrapper : IWin32Window
{
	public IntPtr Handle { get; }

	public MtWindowWrapper(IntPtr handle)
	{
		Handle = handle;
	}
}
