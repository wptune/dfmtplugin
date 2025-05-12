using System;
using Newtonsoft.Json;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace CustomMTTranslationProvider;

public class MtTranslationProvider : ITranslationProvider
{
	public MtTranslationOptions Options { get; set; }

	public bool IsReadOnly => false;

	public string Name
	{
		get
		{
			string text = Storage.TranslationProviderInfoNamePrefix;
			if (Storage.ExtractModelFromUrl(Uri.AbsoluteUri) != "")
			{
				text = text + " <" + Uri.UnescapeDataString(Storage.ExtractModelFromUrl(Uri.AbsoluteUri)).Replace("+", " ") + ">";
			}
			return text;
		}
	}

	public ProviderStatusInfo StatusInfo => new ProviderStatusInfo(true, Storage.TranslationProviderLabel);

	public bool SupportsConcordanceSearch { get; }

	public bool SupportsDocumentSearches { get; }

	public bool SupportsFilters { get; }

	public bool SupportsFuzzySearch => false;

	public bool SupportsMultipleResults => false;

	public bool SupportsPenalties => true;

	public bool SupportsPlaceables => false;

	public bool SupportsScoring => false;

	public bool SupportsSearchForTranslationUnits => true;

	public bool SupportsSourceConcordanceSearch => false;

	public bool SupportsTargetConcordanceSearch => false;

	public bool SupportsStructureContext { get; }

	public bool SupportsTaggedInput => true;

	public bool SupportsTranslation => true;

	public bool SupportsUpdate => false;

	public bool SupportsWordCounts => false;

	public TranslationMethod TranslationMethod => MtTranslationOptions.ProviderTranslationMethod;

	public Uri Uri => Options.UriBuilder.Uri;

	public MtTranslationProvider(MtTranslationOptions options)
	{
		Options = options;
	}

	public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
	{
		return (ITranslationProviderLanguageDirection)(object)new MtTranslationProviderLanguageDirection(this, languageDirection, Options);
	}

	public void LoadState(string translationProviderState)
	{
		Options = JsonConvert.DeserializeObject<MtTranslationOptions>(translationProviderState);
	}

	public void SetOptions()
	{
	}

	public void RefreshStatusInfo()
	{
	}

	public string SerializeState()
	{
		return JsonConvert.SerializeObject((object)Options);
	}

	public bool SupportsLanguageDirection(LanguagePair languageDirection)
	{
		return true;
	}
}
