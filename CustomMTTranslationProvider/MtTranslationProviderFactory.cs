using System;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace CustomMTTranslationProvider;

[TranslationProviderFactory(Id = "CMTTranslationProviderFactory", Name = "CMTTranslationProviderFactory", Description = "Custom.MT Translation Provider")]
public class MtTranslationProviderFactory : ITranslationProviderFactory
{
	public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
	{
		if (!SupportsTranslationProviderUri(translationProviderUri))
		{
			throw new Exception(Storage.UriNotSupportedException);
		}
		string userToken = Storage.ExtractTokenFromUrl(translationProviderUri.AbsoluteUri);
		return (ITranslationProvider)(object)new MtTranslationProvider(new MtTranslationOptions(Storage.ExtractModelFromUrl(translationProviderUri.AbsoluteUri), userToken));
	}

	public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		return new TranslationProviderInfo
		{
			TranslationMethod = MtTranslationOptions.ProviderTranslationMethod,
			Name = Storage.TranslationProviderInfoNamePrefix + " <" + Storage.ExtractModelFromUrl(translationProviderUri.AbsoluteUri).Replace("+", " ") + ">"
		};
	}

	public bool SupportsTranslationProviderUri(Uri translationProviderUri)
	{
		if (translationProviderUri == null)
		{
			throw new ArgumentNullException("translationProviderUri");
		}
		return string.Equals(translationProviderUri.Scheme, Storage.Urischeme, StringComparison.OrdinalIgnoreCase);
	}
}
