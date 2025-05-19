using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace MicrosoftTranslatorProvider.Helpers;

public static class Log
{
	public static void Setup()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		if (LogManager.Configuration == null)
		{
			LogManager.Configuration = new LoggingConfiguration();
		}
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PluginResources.LogsFolderPath, PluginResources.AppLogFolder);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		FileTarget val = new FileTarget
		{
			Name = "MicrosoftTranslatorProvider",
			FileName = Layout.op_Implicit(Path.Combine(text, PluginResources.LogsFileName)),
			Layout = Layout.op_Implicit("${logger}: ${longdate} ${level} ${message}  ${exception}")
		};
		LogManager.Configuration.AddTarget((Target)(object)val);
		LogManager.Configuration.AddRuleForAllLevels((Target)(object)val, "*MicrosoftTranslatorProvider*");
		LogManager.ReconfigExistingLoggers();
	}
}
