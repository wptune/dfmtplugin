using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using LanguageMappingProvider.Database;
using LanguageMappingProvider.Model;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Studio.TranslationProvider;
using Sdl.Core.Globalization;
using Sdl.Core.Globalization.LanguageRegistry;

namespace MicrosoftTranslatorProvider.Extensions;

public static class DatabaseExtensions
{
	public static string GetLanguageCode(this CultureInfo cultureInfo)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		Regex regex = new Regex("^(.*?)\\s*(?:\\((.*?)\\))?$");
		Match match = regex.Match(cultureInfo.DisplayName);
		string languageName = match.Groups[1].Value;
		string languageRegion = (match.Groups[2].Success ? match.Groups[2].Value : null);
		LanguageMappingDatabase val = new LanguageMappingDatabase("microsoft", (IList<LanguageMapping>)null);
		IEnumerable<LanguageMapping> mappedLanguages = val.GetMappedLanguages();
		LanguageMapping val2 = mappedLanguages.FirstOrDefault((Func<LanguageMapping, bool>)((LanguageMapping x) => x.Name == languageName && x.Region == languageRegion)) ?? mappedLanguages.FirstOrDefault((Func<LanguageMapping, bool>)((LanguageMapping x) => x.TradosCode == cultureInfo.Name));
		return val2.LanguageCode;
	}

	public static List<LanguageMapping> GetDefaultMapping(ITranslationOptions translationOptions)
	{
		MicrosoftApi microsoftApi = new MicrosoftApi(translationOptions);
		IEnumerable<LanguageMapping> enumerable = from x in microsoftApi.GetSupportedLanguages()
			where !x.Name.Contains("Chinese")
			select x;
		Regex regex = new Regex("^(.*?)\\s*(?:\\((.*?)\\))?$");
		foreach (LanguageMapping item in enumerable.Where((LanguageMapping x) => x.Name.Contains("(")))
		{
			if (!item.Name.Contains("Chinese"))
			{
				Match match = regex.Match(item.Name);
				item.Name = match.Groups[1].Value;
				item.Region = match.Groups[2].Value;
			}
		}
		return (from x in enumerable.Union(CreateChineseMapping())
			orderby x.Name, x.Region
			select x).ToList();
	}

	public static void CreateDatabase(ITranslationOptions translationOptions)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<LanguageMapping> defaultMapping = GetDefaultMapping(translationOptions);
		new LanguageMappingDatabase("microsoft", (IList<LanguageMapping>)defaultMapping);
	}

	private static List<LanguageMapping> CreateChineseMapping()
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		List<Language> list = (from x in LanguageRegistryApi.Instance.GetAllLanguages()
			where ((LanguageData)x).EnglishName.StartsWith("Chinese")
			select x).ToList();
		List<LanguageMapping> list2 = new List<LanguageMapping>();
		foreach (Language item in list)
		{
			Regex regex = new Regex("^(.*?)\\s*(?:\\((.*?)\\))?$");
			Match match = regex.Match(item.DisplayName);
			string languageName = match.Groups[1].Value;
			string languageRegion = (match.Groups[2].Success ? match.Groups[2].Value : null);
			if (!list2.Any((LanguageMapping x) => x.Name == languageName && x.Region == languageRegion) && languageRegion != null)
			{
				list2.Add(new LanguageMapping
				{
					Name = languageName,
					Region = languageRegion,
					LanguageCode = "zh-" + ((LanguageData)item).Script
				});
			}
		}
		return list2;
	}
}
