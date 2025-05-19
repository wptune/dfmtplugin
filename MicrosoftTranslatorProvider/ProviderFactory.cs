using System;
using System.Collections.Generic;
using System.Net;
using MicrosoftTranslatorProvider.Model;
using Newtonsoft.Json;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace MicrosoftTranslatorProvider;

[TranslationProviderFactory(Id = "MicrosoftTranslatorProviderPlugin_Factory", Name = "MicrosoftTranslatorProviderPlugin_Factory", Description = "MicrosoftTranslatorProviderPlugin_Factory")]
public class ProviderFactory : ITranslationProviderFactory
{
	public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		if (!SupportsTranslationProviderUri(translationProviderUri))
		{
			throw new Exception(PluginResources.UriNotSupportedMessage);
		}
		ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
		TranslationProviderCredential val = credentialStore.GetCredential(new Uri("microsofttranslatorprovider:///")) ?? throw new TranslationProviderAuthenticationException();
		TranslationOptions translationOptions = JsonConvert.DeserializeObject<TranslationOptions>(translationProviderState);
		List<UrlMetadata> list = new List<UrlMetadata>();
		try
		{
			GenericCredentials val2 = new GenericCredentials(val.Credential);
			string[] propertyKeys = val2.GetPropertyKeys();
			foreach (string text in propertyKeys)
			{
				if (text.StartsWith("header_"))
				{
					list.Add(new UrlMetadata
					{
						Key = text.Replace("header_", string.Empty),
						Value = val2[text]
					});
				}
			}
			translationOptions.ApiKey = val2["API-Key"];
			TranslationOptions translationOptions2 = translationOptions;
			if (translationOptions2.PrivateEndpoint == null)
			{
				string text3 = (translationOptions2.PrivateEndpoint = val2["PrivateEndpoint"]);
			}
		}
		catch
		{
		}
		return (ITranslationProvider)(object)new Provider(translationOptions)
		{
			PrivateHeaders = list
		};
	}

	public bool SupportsTranslationProviderUri(Uri translationProviderUri)
	{
		if ((object)translationProviderUri == null)
		{
			throw new ArgumentNullException(PluginResources.UriNotSupportedMessage);
		}
		return string.Equals(translationProviderUri.Scheme, "microsofttranslatorprovider", StringComparison.OrdinalIgnoreCase);
	}

	public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		return new TranslationProviderInfo
		{
			TranslationMethod = TranslationOptions.ProviderTranslationMethod,
			Name = PluginResources.Plugin_NiceName
		};
	}
}
