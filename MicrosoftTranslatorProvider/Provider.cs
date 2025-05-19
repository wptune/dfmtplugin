using System;
using System.Collections.Generic;
using System.Linq;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Interface;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using MicrosoftTranslatorProvider.Studio.TranslationProvider;
using Newtonsoft.Json;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace MicrosoftTranslatorProvider;

public class Provider : ITranslationProvider, ITranslationProviderExtension
{
	private MicrosoftApi _providerConnector;

	public string Name
	{
		get
		{
			string customProviderName = Options.CustomProviderName;
			bool useCustomProviderName = Options.UseCustomProviderName;
			return customProviderName.SetProviderName(useCustomProviderName);
		}
	}

	public List<UrlMetadata> PrivateHeaders { get; set; }

	public RegionsProvider RegionsProvider { get; }

	public ITranslationOptions Options { get; set; }

	public Uri Uri => Options.Uri;

	public ProviderStatusInfo StatusInfo => new ProviderStatusInfo(true, PluginResources.Microsoft_NiceName);

	public TranslationMethod TranslationMethod => (TranslationMethod)2;

	public bool IsReadOnly => true;

	public bool SupportsConcordanceSearch => false;

	public bool SupportsDocumentSearches => false;

	public bool SupportsFilters => false;

	public bool SupportsFuzzySearch => false;

	public bool SupportsMultipleResults => false;

	public bool SupportsPenalties => true;

	public bool SupportsPlaceables => false;

	public bool SupportsScoring => false;

	public bool SupportsSearchForTranslationUnits => true;

	public bool SupportsSourceConcordanceSearch => false;

	public bool SupportsStructureContext => false;

	public bool SupportsTaggedInput => true;

	public bool SupportsTargetConcordanceSearch => false;

	public bool SupportsTranslation => true;

	public bool SupportsUpdate => false;

	public bool SupportsWordCounts => false;

	public Dictionary<string, string> LanguagesSupported { get; set; } = new Dictionary<string, string>();


	public Provider(ITranslationOptions options)
	{
		Options = options;
		LanguagesSupported = Options.LanguagesSupported.ToDictionary((string lang) => lang, (string lang) => PluginResources.Microsoft_ShortName);
	}

	public bool SupportsLanguageDirection(LanguagePair languageDirection)
	{
		if (Options.UsePrivateEndpoint)
		{
			return true;
		}
		if (_providerConnector == null)
		{
			_providerConnector = new MicrosoftApi(Options.ApiKey, Options.Region);
		}
		_providerConnector.ResetCredentials(Options.ApiKey, Options.Region);
		return _providerConnector.IsSupportedLanguagePair(languageDirection.SourceCulture.Name, languageDirection.TargetCulture.Name);
	}

	public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
	{
		return (ITranslationProviderLanguageDirection)(object)new ProviderLanguageDirection(this, languageDirection);
	}

	public string SerializeState()
	{
		return JsonConvert.SerializeObject((object)Options);
	}

	public void LoadState(string translationProviderState)
	{
		Options = JsonConvert.DeserializeObject<TranslationOptions>(translationProviderState);
	}

	public void RefreshStatusInfo()
	{
	}
}
