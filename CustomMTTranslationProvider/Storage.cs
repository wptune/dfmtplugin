using System;
using System.Collections.Generic;

namespace CustomMTTranslationProvider;

public class Storage
{
	public class SettingsFileData
	{
		public string token;

		public string source;

		public string target;

		public string model;
	}

	public class GetTemplateListPayload
	{
		public string source_language;

		public string target_language;
	}

	public class GetTemplatesResult
	{
		public string template_name;
	}

	public class GetTranslationPayload
	{
		public string[] text;

		public string template_name;
	}

	public class GetTranslationResult
	{
		public List<string> translated_text;

		public string message;
	}

	public class Error
	{
		public string error;
	}

	public static string TranslationProviderInfoNamePrefix = "Custom.MT Translation";

	public static string TranslationProviderLabel = "Custom.MT Translation";

	public static string Description = "Translate using Custom.MT";

	public static string Urischeme = "cmttranslateprovider";

	public static string TokenProperty = "tokenTag";

	public static string modelProperty = "modelTag";

	private static string API_Endpoint = "https://console.custom.mt";

	public static string UriNotSupportedException = "Uri not supported";

	public static string urlGetTempplates => API_Endpoint + "/translation/get-templates";

	public static string urlGetTranslation => API_Endpoint + "/translation/translate";

	public static string SettingsFile()
	{
		return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CMT\\settings.json";
	}

	public static string ExtractTokenFromUrl(string url)
	{
		if (!url.Contains(TokenProperty))
		{
			return "";
		}
		string text = url.Substring(url.IndexOf(TokenProperty) + TokenProperty.Length + 1);
		if (text.Contains(modelProperty))
		{
			text = text.Substring(0, text.IndexOf("&"));
		}
		return text;
	}

	public static string ExtractModelFromUrl(string url)
	{
		if (!url.Contains(modelProperty))
		{
			return "";
		}
		string text = url.Substring(url.IndexOf(modelProperty) + modelProperty.Length + 1);
		if (text.Contains(TokenProperty))
		{
			text = text.Substring(0, text.IndexOf("&"));
		}
		return text;
	}
}
