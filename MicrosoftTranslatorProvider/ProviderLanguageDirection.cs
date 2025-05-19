using System;
using System.Collections.Generic;
using MicrosoftTranslatorProvider.ApiService;
using MicrosoftTranslatorProvider.Helpers;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using MicrosoftTranslatorProvider.Studio.TranslationProvider;
using Sdl.Core.Globalization;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace MicrosoftTranslatorProvider;

public class ProviderLanguageDirection : ITranslationProviderLanguageDirection
{
	private readonly ITranslationOptions _options;

	private readonly LanguagePair _languagePair;

	private readonly Provider _provider;

	private MicrosoftApi _providerConnecter;

	private PrivateEndpointApi _privateEndpoint;

	private MicrosoftSegmentEditor _postLookupSegmentEditor;

	private MicrosoftSegmentEditor _preLookupSegmentEditor;

	private TranslationUnit _inputTu;

	public ITranslationProvider TranslationProvider => (ITranslationProvider)(object)_provider;

	public bool CanReverseLanguageDirection => false;

	CultureCode ITranslationProviderLanguageDirection.SourceLanguage => _languagePair.SourceCulture;

	CultureCode ITranslationProviderLanguageDirection.TargetLanguage => _languagePair.TargetCulture;

	public ProviderLanguageDirection(Provider provider, LanguagePair languagePair)
	{
		_provider = provider;
		_options = _provider.Options;
		_languagePair = languagePair;
	}

	public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
	{
		SearchResults[] array = (SearchResults[])(object)new SearchResults[segments.Length];
		for (int i = 0; i < segments.Length; i++)
		{
			array[i] = SearchSegment(settings, segments[i]);
		}
		return array;
	}

	public SearchResults SearchSegment(SearchSettings settings, Segment segment)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		Segment val = new Segment(_languagePair.TargetCulture);
		SearchResults val2 = new SearchResults
		{
			SourceSegment = segment.Duplicate()
		};
		if (!_options.ResendDrafts && (int)_inputTu.ConfirmationLevel > 0)
		{
			val.Add(PluginResources.TranslationLookupDraftNotResentMessage);
			val2.Add(CreateSearchResult(segment, val));
			return val2;
		}
		Segment val3 = segment.Duplicate();
		if (_options.SendPlainTextOnly || !val3.HasTags)
		{
			val.Add(SearchSegmentOnTextOnly(val3));
			val2.Add(CreateSearchResult(val3, val));
			return val2;
		}
		if (_options.UsePreEdit)
		{
			if (_preLookupSegmentEditor == null)
			{
				_preLookupSegmentEditor = new MicrosoftSegmentEditor(_options.PreLookupFilename);
			}
			val3 = GetEditedSegment(_preLookupSegmentEditor, val3);
		}
		TagPlacer tagPlacer = new TagPlacer(val3);
		string returnedText = Lookup(tagPlacer.PreparedSourceText, _options);
		val = tagPlacer.GetTaggedSegment(returnedText).Duplicate();
		if (_options.UsePostEdit)
		{
			if (_postLookupSegmentEditor == null)
			{
				_postLookupSegmentEditor = new MicrosoftSegmentEditor(_options.PostLookupFilename);
			}
			val = GetEditedSegment(_postLookupSegmentEditor, val);
		}
		val2.Add(CreateSearchResult(val3, val));
		return val2;
	}

	public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
	{
		if (segments == null || mask == null)
		{
			throw new ArgumentNullException("null in SearchSegmentsMasked");
		}
		if (mask.Length != segments.Length)
		{
			throw new ArgumentException("length SearchSegmentsMasked");
		}
		SearchResults[] array = (SearchResults[])(object)new SearchResults[segments.Length];
		for (int i = 0; i < segments.Length; i++)
		{
			if (mask[i])
			{
				array[i] = SearchSegment(settings, segments[i]);
			}
			else
			{
				array[i] = null;
			}
		}
		return array;
	}

	public SearchResults SearchText(SearchSettings settings, string segment)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Segment val = new Segment(_languagePair.SourceCulture);
		val.Add(segment);
		return SearchSegment(settings, val);
	}

	public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
	{
		_inputTu = translationUnit;
		return SearchSegment(settings, translationUnit.SourceSegment);
	}

	public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
	{
		SearchResults[] array = (SearchResults[])(object)new SearchResults[translationUnits.Length];
		for (int i = 0; i < translationUnits.Length; i++)
		{
			if (translationUnits[i] != null)
			{
				_inputTu = translationUnits[i];
				array[i] = SearchSegment(settings, translationUnits[i].SourceSegment);
			}
		}
		return array;
	}

	public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
	{
		List<SearchResults> list = new List<SearchResults>(mask.Length);
		for (int i = 0; i < translationUnits.Length; i++)
		{
			if (mask[i])
			{
				list.Add(SearchTranslationUnit(settings, translationUnits[i]));
			}
			else
			{
				list.Add(null);
			}
		}
		return list.ToArray();
	}

	private string SearchSegmentOnTextOnly(Segment segment)
	{
		string text = segment.ToPlain();
		if (_options.UsePreEdit)
		{
			if (_preLookupSegmentEditor == null)
			{
				_preLookupSegmentEditor = new MicrosoftSegmentEditor(_options.PreLookupFilename);
			}
			text = GetEditedString(_preLookupSegmentEditor, text);
			segment.Clear();
			segment.Add(text);
		}
		string text2 = Lookup(text, _options);
		if (_options.UsePostEdit)
		{
			if (_postLookupSegmentEditor == null)
			{
				_postLookupSegmentEditor = new MicrosoftSegmentEditor(_options.PostLookupFilename);
			}
			text2 = GetEditedString(_postLookupSegmentEditor, text2);
		}
		return text2;
	}

	private SearchResult CreateSearchResult(Segment searchSegment, Segment translation)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0056: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		TranslationUnit val = new TranslationUnit
		{
			SourceSegment = searchSegment.Duplicate(),
			TargetSegment = translation,
			Origin = (TranslationUnitOrigin)6
		};
		((PersistentObject)val).ResourceId = new PersistentObjectToken(((object)val).GetHashCode(), Guid.Empty);
		SearchResult val2 = new SearchResult(val)
		{
			ScoringResult = new ScoringResult
			{
				BaseScore = 0
			}
		};
		val.ConfirmationLevel = (ConfirmationLevel)1;
		val2.TranslationProposal = new TranslationUnit(val);
		return val2;
	}

	private string GetEditedString(MicrosoftSegmentEditor editor, string sourcetext)
	{
		return editor.EditText(sourcetext);
	}

	private Segment GetEditedSegment(MicrosoftSegmentEditor editor, Segment inSegment)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Segment val = new Segment(inSegment.Culture);
		foreach (SegmentElement element in inSegment.Elements)
		{
			if (((object)element).GetType() == typeof(Tag))
			{
				val.Add(element);
				continue;
			}
			string text = editor.EditText(((object)element).ToString());
			val.Add(text);
		}
		return val;
	}

	private string Lookup(string sourcetext, ITranslationOptions options)
	{
		string sourceLanguage = ((object)_languagePair.SourceCulture).ToString();
		string targetLanguage = ((object)_languagePair.TargetCulture).ToString();
		if (options.UsePrivateEndpoint)
		{
			_privateEndpoint = new PrivateEndpointApi(options.PrivateEndpoint, _provider.PrivateHeaders, options.Parameters);
			return _privateEndpoint.Translate(sourceLanguage, targetLanguage, sourcetext);
		}
		MicrosoftApi providerConnecter = _providerConnecter;
		MicrosoftApi microsoftApi = providerConnecter;
		if (microsoftApi == null)
		{
			_providerConnecter = new MicrosoftApi(_options);
		}
		else
		{
			_providerConnecter.ResetCredentials(options.ApiKey, options.Region);
		}
		return _providerConnecter.Translate(_languagePair, sourcetext);
	}

	public ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)
	{
		throw new NotImplementedException();
	}

	public ImportResult[] AddOrUpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, bool[] mask)
	{
		throw new NotImplementedException();
	}

	public ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)
	{
		throw new NotImplementedException();
	}

	public ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)
	{
		throw new NotImplementedException();
	}

	public ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask)
	{
		throw new NotImplementedException();
	}

	public ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)
	{
		throw new NotImplementedException();
	}

	public ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits)
	{
		throw new NotImplementedException();
	}
}
