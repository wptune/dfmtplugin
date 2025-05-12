using System;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace CustomMTTranslationProvider;

public class MtTranslationOptions
{
	public static readonly TranslationMethod ProviderTranslationMethod = (TranslationMethod)2;

	public TranslationProviderUriBuilder UriBuilder;

	public void SetModel(string model)
	{
		UriBuilder[Storage.modelProperty] = model;
	}

	public string GetModel()
	{
		if (string.IsNullOrEmpty(UriBuilder[Storage.modelProperty]))
		{
			return "";
		}
		return Uri.UnescapeDataString(UriBuilder[Storage.modelProperty]).Replace("+", " ");
	}

	public void SetToken(string token)
	{
		UriBuilder[Storage.TokenProperty] = token;
	}

	public string GetToken()
	{
		if (string.IsNullOrEmpty(UriBuilder[Storage.TokenProperty]))
		{
			return "";
		}
		return Uri.UnescapeDataString(UriBuilder[Storage.TokenProperty]);
	}

	public MtTranslationOptions(string modelId, string userToken)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		UriBuilder = new TranslationProviderUriBuilder(Storage.Urischeme);
		SetModel(modelId);
		SetToken(userToken);
	}

	public MtTranslationOptions()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		UriBuilder = new TranslationProviderUriBuilder(Storage.Urischeme);
	}
}
