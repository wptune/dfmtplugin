using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Helpers;
using MicrosoftTranslatorProvider.Model;
using Newtonsoft.Json;

namespace MicrosoftTranslatorProvider.ApiService;

internal class PrivateEndpointApi
{
	private readonly string _uri;

	private readonly List<UrlMetadata> _headers;

	private readonly List<UrlMetadata> _parameters;

	public PrivateEndpointApi(string endpoint, List<UrlMetadata> headers, List<UrlMetadata> parameters)
	{
		_headers = headers;
		_parameters = parameters;
		_uri = BuildUri(endpoint);
	}

	private string BuildUri(string endpoint)
	{
		string text = (endpoint.StartsWith("https://") ? endpoint : ("https://" + endpoint));
		text += (text.EndsWith("?") ? string.Empty : "?");
		foreach (UrlMetadata parameter in _parameters)
		{
			if (!parameter.Key.Equals("from") && !parameter.Key.Equals("to"))
			{
				text = text + parameter.Key + "=" + parameter.Value + "&";
			}
		}
		return text.EndsWith("?") ? text : text.Substring(0, text.Length - 1);
	}

	public string Translate(string sourceLanguage, string targetLanguage, string textToTranslate)
	{
		try
		{
			sourceLanguage = ConvertLanguageCode(sourceLanguage);
			targetLanguage = ConvertLanguageCode(targetLanguage);
			return TryTranslate(sourceLanguage, targetLanguage, textToTranslate);
		}
		catch (WebException exception)
		{
			ErrorHandler.HandleError(exception);
			return null;
		}
	}

	private string TryTranslate(string sourceLanguage, string targetLanguage, string textToTranslate)
	{
		MatchCollection matchCollection = new Regex("(\\<\\w+[üäåëöøßşÿÄÅÆĞ]*[^\\d\\W\\\\/\\\\]+\\>)").Matches(textToTranslate);
		if (matchCollection.Count > 0)
		{
			textToTranslate = textToTranslate.ReplaceCharacters(matchCollection);
		}
		return RequestTranslation(sourceLanguage, targetLanguage, textToTranslate);
	}

	private string RequestTranslation(string sourceLanguage, string targetLanguage, string textToTranslate)
	{
		try
		{
			return TryRequestTranslation(sourceLanguage, targetLanguage, textToTranslate);
		}
		catch (Exception exception)
		{
			ErrorHandler.HandleError(exception);
			return null;
		}
	}

	private string TryRequestTranslation(string sourceLanguage, string targetLanguage, string textToTranslate)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		object[] array = new object[1]
		{
			new
			{
				Text = textToTranslate
			}
		};
		string text = JsonConvert.SerializeObject((object)array);
		HttpRequestMessage val = new HttpRequestMessage();
		val.Method = HttpMethod.Post;
		val.Content = (HttpContent)new StringContent(text, Encoding.UTF8, "application/json");
		val.RequestUri = new Uri(_uri + "from=" + sourceLanguage + "&to=" + targetLanguage);
		HttpRequestMessage val2 = val;
		foreach (UrlMetadata header in _headers)
		{
			((HttpHeaders)val2.Headers).Add(header.Key, header.Value);
		}
		HttpClient val3 = new HttpClient();
		HttpResponseMessage result = val3.SendAsync(val2).Result;
		string result2 = result.Content.ReadAsStringAsync().Result;
		if (!result.IsSuccessStatusCode)
		{
			ResponseMessage responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(result2);
			throw new Exception(responseMessage.Error.Message);
		}
		List<TranslationResponse> list = JsonConvert.DeserializeObject<List<TranslationResponse>>(result2);
		return new HtmlUtil().HtmlDecode(list[0]?.Translations[0]?.Text);
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
}
