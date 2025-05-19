using Microsoft.Win32;
using MicrosoftTranslatorProvider.Interfaces;

namespace MicrosoftTranslatorProvider.Helpers;

public class OpenFileDialogService : IOpenFileDialogService
{
	public string ShowDialog(string filter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		OpenFileDialog val = new OpenFileDialog
		{
			Filter = filter,
			Multiselect = false
		};
		return ((CommonDialog)val).ShowDialog().Value ? ((FileDialog)val).FileName : string.Empty;
	}
}
