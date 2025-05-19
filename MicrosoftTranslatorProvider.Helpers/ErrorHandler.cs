using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NLog;

namespace MicrosoftTranslatorProvider.Helpers;

public static class ErrorHandler
{
	private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

	public static void HandleError(string errorMessage, string source)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		MessageBox.Show(errorMessage, source, (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0);
	}

	public static void HandleError(Exception exception)
	{
		_logger.Error($"{MethodBase.GetCurrentMethod().Name}: {exception}");
		HandleError("An unexpected error occured.\nThe error was logged at " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PluginResources.LogsFolderPath, PluginResources.AppLogFolder) + ".\n\n" + exception.Message, "Unexpected error");
	}
}
