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

    public void SetBaseUrl(string baseurl)
    {
        UriBuilder[Storage.BaseUrlProperty] = baseurl;
    }

    public string GetBaseUrl()
    {
        if (string.IsNullOrEmpty(UriBuilder[Storage.BaseUrlProperty]))
        {
            return "";
        }
        return Uri.UnescapeDataString(UriBuilder[Storage.BaseUrlProperty]);
    }

    public void SetPath(string path)
    {
        UriBuilder[Storage.PathProperty] = path;
    }

    public string GetPath()
    {
        if (string.IsNullOrEmpty(UriBuilder[Storage.PathProperty]))
        {
            return "";
        }
        return Uri.UnescapeDataString(UriBuilder[Storage.PathProperty]);
    }

    public void SetApiKey(string apikey)
    {
        UriBuilder[Storage.ApiKeyProperty] = apikey;
    }

    public string GetApiKey()
    {
        if (string.IsNullOrEmpty(UriBuilder[Storage.ApiKeyProperty]))
        {
            return "";
        }
        return Uri.UnescapeDataString(UriBuilder[Storage.ApiKeyProperty]);
    }

    public void SetSubject(string subject)
    {
        UriBuilder[Storage.SubjectProperty] = subject;
    }

    public string GetSubject()
    {
        if (string.IsNullOrEmpty(UriBuilder[Storage.SubjectProperty]))
        {
            return "";
        }
        return Uri.UnescapeDataString(UriBuilder[Storage.SubjectProperty]);
    }

    public void SetStyleguide(string styleguide)
    {
        UriBuilder[Storage.StyleguideProperty] = styleguide;
    }

    public string GetStyleguide()
    {
        if (string.IsNullOrEmpty(UriBuilder[Storage.StyleguideProperty]))
        {
            return "";
        }
        return Uri.UnescapeDataString(UriBuilder[Storage.StyleguideProperty]);
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
