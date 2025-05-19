using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using LanguageMappingProvider.Model;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Helpers;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using NLog;
using Newtonsoft.Json;
using RestSharp;
using Sdl.LanguagePlatform.Core;

namespace MicrosoftTranslatorProvider.Studio.TranslationProvider;

public class MicrosoftApi
{
	private readonly HtmlUtil _htmlUtil = new HtmlUtil();

	private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	private readonly ITranslationOptions _options;

	private string _region;

	private string _subscriptionKey;

	private HashSet<string> _supportedLanguages;

	public MicrosoftApi(ITranslationOptions options)
	{
		_options = options;
		_subscriptionKey = options.ApiKey;
		_region = options.Region;
		SetSupportedLanguages();
	}

	public MicrosoftApi(string subscriptionKey, string region)
	{
		_subscriptionKey = subscriptionKey;
		_region = region;
		SetSupportedLanguages();
	}

	public List<LanguageMapping> GetSupportedLanguages()
	{
		try
		{
			return TryGetSupportedLanguages();
		}
		catch (Exception exception)
		{
			ErrorHandler.HandleError(exception);
			return null;
		}
	}

	public bool IsSupportedLanguagePair(string sourceLanguage, string tarrgetLanguage)
	{
		string equalValue = ConvertLanguageCode(sourceLanguage);
		string equalValue2 = ConvertLanguageCode(tarrgetLanguage);
		string actualValue;
		bool flag = _supportedLanguages.TryGetValue(equalValue, out actualValue);
		bool flag2 = _supportedLanguages.TryGetValue(equalValue2, out actualValue);
		return flag && flag2;
	}

	public void ResetCredentials(string subscriptionKey, string region)
	{
		if (!(subscriptionKey == _subscriptionKey) || !(region == _region))
		{
			_subscriptionKey = subscriptionKey;
			_region = region;
			SetSupportedLanguages();
		}
	}

	public string Translate(LanguagePair languagepair, string textToTranslate)
	{
		try
		{
			PairMapping pairMapping = _options.LanguageMappings.FirstOrDefault((PairMapping x) => x.LanguagePair.TargetCultureName == languagepair.TargetCultureName);
			string languageCode = new CultureInfo(languagepair.SourceCultureName).GetLanguageCode();
			string languageCode2 = new CultureInfo(languagepair.TargetCultureName).GetLanguageCode();
			string categoryID = (string.IsNullOrEmpty(pairMapping.CategoryID) ? "general" : pairMapping.CategoryID);
			return TryTranslate(languageCode, languageCode2, textToTranslate, categoryID);
		}
		catch (WebException exception)
		{
			ErrorHandler.HandleError(exception);
			return null;
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("The category parameter is invalid"))
			{
				ErrorHandler.HandleError("The Category ID is not valid for this language pair", "Category ID");
				return null;
			}
			ErrorHandler.HandleError(ex);
			return null;
		}
	}

	private string BuildTranslationUri(string sourceLanguage, string targetLanguage, string category)
	{
		string text = "&from=" + sourceLanguage + "&to=" + targetLanguage + "&textType=html&category=" + category;
		return "https://api.cognitive.microsofttranslator.com" + "/translate?api-version=3.0" + text;
	}

	private string ConvertLanguageCode(string languageCode)
	{
		CultureInfo cultureInfo = new CultureInfo(languageCode);
		bool flag = languageCode.Contains("sr-Cyrl");
		bool flag2 = languageCode.Contains("sr-Latn");
		bool flag3 = "zh-TW zh-HK zh-MO zh-Hant zh-CHT".Contains(cultureInfo.Name);
		bool flag4 = "zh-CN zh-SG zh-Hans-HK zh-Hans-MO zh-Hans zh-CHS".Contains(cultureInfo.Name);
		if (flag)
		{
			return "sr-Cyrl";
		}
		if (flag2)
		{
			return "sr-Latn";
		}
		if (flag3)
		{
			return "zh-Hant";
		}
		if (flag4)
		{
			return "zh-Hans";
		}
		return cultureInfo.TwoLetterISOLanguageName;
	}

	private string RequestTranslation(string sourceLanguage, string targetLanguage, string textToTranslate, string categoryID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		object[] array = new object[1]
		{
			new
			{
				Text = textToTranslate
			}
		};
		string text = JsonConvert.SerializeObject((object)array);
		HttpRequestMessage val = new HttpRequestMessage
		{
			Method = HttpMethod.Post,
			Content = (HttpContent)new StringContent(text, Encoding.UTF8, "application/json"),
			RequestUri = new Uri(BuildTranslationUri(sourceLanguage, targetLanguage, categoryID))
		};
		((HttpHeaders)val.Headers).Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
		((HttpHeaders)val.Headers).Add("Ocp-Apim-Subscription-Region", _region);
		HttpClient val2 = new HttpClient();
		HttpResponseMessage result = val2.SendAsync(val).Result;
		string result2 = result.Content.ReadAsStringAsync().Result;
		if (!result.IsSuccessStatusCode)
		{
			ResponseMessage responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(result2);
			throw new Exception(responseMessage.Error.Message);
		}
		List<TranslationResponse> list = JsonConvert.DeserializeObject<List<TranslationResponse>>(result2);
		return _htmlUtil.HtmlDecode(list[0]?.Translations[0]?.Text);
	}

	private void SetSupportedLanguages()
	{
		try
		{
			TrySetSupportedLanguages();
		}
		catch (Exception exception)
		{
			ErrorHandler.HandleError(exception);
		}
	}

	private List<LanguageMapping> TryGetSupportedLanguages()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		Uri uri = new Uri("https://api.cognitive.microsofttranslator.com");
		RestClient val = new RestClient(uri, (ConfigureRestClient)null, (ConfigureHeaders)null, (ConfigureSerialization)null, false);
		RestRequest val2 = new RestRequest("languages", (Method)0);
		RestRequestExtensions.AddParameter(val2, "api-version", "3.0", true);
		RestRequestExtensions.AddParameter(val2, "scope", "translation", true);
		RestResponse result = val.ExecuteAsync(val2, default(CancellationToken)).Result;
		if (!((RestResponseBase)result).IsSuccessful)
		{
			throw new HttpException($"Error: {((RestResponseBase)result).StatusCode}, {((RestResponseBase)result).StatusDescription}");
		}
		IEnumerable<KeyValuePair<string, LanguageDetails>> enumerable = JsonConvert.DeserializeObject<LanguageResponse>(((RestResponseBase)result).Content)?.Translation?.Distinct();
		List<LanguageMapping> list = new List<LanguageMapping>();
		foreach (KeyValuePair<string, LanguageDetails> item in enumerable)
		{
			list.Add(new LanguageMapping
			{
				Name = item.Value.Name,
				LanguageCode = item.Key
			});
		}
		return list;
	}

	private void TrySetSupportedLanguages()
	{
		List<LanguageMapping> supportedLanguages = GetSupportedLanguages();
		_supportedLanguages = new HashSet<string>();
		foreach (LanguageMapping item in supportedLanguages)
		{
			_supportedLanguages.Add(item.LanguageCode);
		}
	}

	private string TryTranslate(string sourceLanguage, string targetLanguage, string textToTranslate, string categoryID)
	{
		MatchCollection matchCollection = new Regex("(\\<\\w+[üäåëöøßşÿÄÅÆĞ]*[^\\d\\W\\\\/\\\\]+\\>)").Matches(textToTranslate);
		if (matchCollection.Count > 0)
		{
			textToTranslate = textToTranslate.ReplaceCharacters(matchCollection);
		}
		return RequestTranslation(sourceLanguage, targetLanguage, textToTranslate, categoryID);
	}
}
