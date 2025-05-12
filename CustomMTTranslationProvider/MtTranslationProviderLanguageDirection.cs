using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CustomMTTranslation;
using CustomMTTranslationProvider.CustomMT;
using Sdl.Core.Globalization;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace CustomMTTranslationProvider;

public class MtTranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
{
	private readonly MtTranslationProvider _provider;

	private readonly LanguagePair _languageDirection;

	public MtTranslationOptions _options;

	private ApiConnecter _CmtConnect;

	public int batchLimit = 49;

	public CultureInfo SourceLanguage => CultureCode.op_Implicit(_languageDirection.SourceCulture);

	public CultureInfo TargetLanguage => CultureCode.op_Implicit(_languageDirection.TargetCulture);

	public ITranslationProvider TranslationProvider => (ITranslationProvider)(object)_provider;

	public bool CanReverseLanguageDirection
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	CultureCode ITranslationProviderLanguageDirection.SourceLanguage
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	CultureCode ITranslationProviderLanguageDirection.TargetLanguage
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public MtTranslationProviderLanguageDirection(MtTranslationProvider provider, LanguagePair languages, MtTranslationOptions options)
	{
		_provider = provider;
		_languageDirection = languages;
		_options = options;
	}

	private string[] LookUpCmt(string[] sourcetext)
	{
		if (_CmtConnect == null)
		{
			_CmtConnect = new ApiConnecter(_options);
		}
		object obj = _CmtConnect.Translate(sourcetext);
		if (obj.GetType() == typeof(Storage.GetTranslationResult))
		{
			return ((Storage.GetTranslationResult)obj).translated_text.ToArray();
		}
		throw new Exception(((Storage.Error)obj).error);
	}

	private SearchResult CreateSearchResult(Segment segment, Segment translation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004d: Expected O, but got Unknown
		TranslationUnit val = new TranslationUnit
		{
			SourceSegment = segment.Duplicate(),
			TargetSegment = translation
		};
		((PersistentObject)val).ResourceId = new PersistentObjectToken(((object)val).GetHashCode(), Guid.Empty);
		val.Origin = (TranslationUnitOrigin)6;
		val.ConfirmationLevel = (ConfirmationLevel)1;
		return new SearchResult(val)
		{
			ScoringResult = new ScoringResult()
		};
	}

	public SearchResults SearchSegment(SearchSettings settings, Segment segment)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Segment val = new Segment(_languageDirection.TargetCulture);
		SearchResults val2 = new SearchResults
		{
			SourceSegment = segment.Duplicate()
		};
		try
		{
			Segment val3 = segment.Duplicate();
			if (_CmtConnect == null)
			{
				_CmtConnect = new ApiConnecter(_options);
			}
			MtTranslationProviderTagPlacer mtTranslationProviderTagPlacer = new MtTranslationProviderTagPlacer(val3);
			string text = LookUpCmt(new string[1] { mtTranslationProviderTagPlacer.PreparedSourceText })[0];
			if (!string.IsNullOrEmpty(text))
			{
				val = mtTranslationProviderTagPlacer.GetTaggedSegment(text).Duplicate();
				val2.Add(CreateSearchResult(val3, val));
				return val2;
			}
			return val2;
		}
		catch
		{
			throw;
		}
	}

	public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
	{
		return SearchSegment(settings, translationUnit.SourceSegment);
	}

	public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
	{
		Segment[] array = (Segment[])(object)new Segment[translationUnits.Length];
		for (int i = 0; i < translationUnits.Length; i++)
		{
			array[i] = translationUnits[i].SourceSegment;
		}
		return SearchSegments(settings, array);
	}

	public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (_CmtConnect == null)
		{
			_CmtConnect = new ApiConnecter(_options);
		}
		SearchResults[] array = (SearchResults[])(object)new SearchResults[segments.Length];
		Segment[] array2 = (Segment[])(object)new Segment[segments.Length];
		MtTranslationProviderTagPlacer[] array3 = new MtTranslationProviderTagPlacer[segments.Length];
		string[] array4 = new string[segments.Length];
		for (int i = 0; i < segments.Length; i++)
		{
			array[i] = new SearchResults
			{
				SourceSegment = segments[i].Duplicate()
			};
			array2[i] = segments[i].Duplicate();
			array3[i] = new MtTranslationProviderTagPlacer(array2[i]);
			array4[i] = array3[i].PreparedSourceText;
		}
		try
		{
			for (int j = 0; j < array2.Length; j += batchLimit)
			{
				string[] sourcetext = array4.Skip(j).Take(batchLimit).ToArray();
				string[] array5 = LookUpCmt(sourcetext);
				for (int k = 0; k < array5.Length; k++)
				{
					if (!string.IsNullOrEmpty(array5[k]))
					{
						Segment translation = array3[j + k].GetTaggedSegment(array5[k]).Duplicate();
						array[j + k].Add(CreateSearchResult(array2[j], translation));
					}
				}
			}
			return array;
		}
		catch
		{
			throw;
		}
	}

	public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
	{
		SearchResults[] array = (SearchResults[])(object)new SearchResults[segments.Length];
		List<Segment> list = new List<Segment>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < segments.Length; i++)
		{
			if (mask == null || mask[i])
			{
				list.Add(segments[i]);
			}
			else
			{
				list2.Add(i);
			}
			array[i] = null;
		}
		SearchResults[] array2 = SearchSegments(settings, list.ToArray());
		for (int j = 0; j < array2.Length; j++)
		{
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == null && !list2.Contains(k))
				{
					array[k] = array2[j];
					break;
				}
			}
		}
		return array;
	}

	public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
	{
		try
		{
			SearchResults[] array = (SearchResults[])(object)new SearchResults[translationUnits.Length];
			List<TranslationUnit> list = new List<TranslationUnit>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < translationUnits.Length; i++)
			{
				if (mask == null || mask[i])
				{
					list.Add(translationUnits[i]);
				}
				else
				{
					list2.Add(i);
				}
				array[i] = null;
			}
			SearchResults[] array2 = SearchTranslationUnits(settings, list.ToArray());
			for (int j = 0; j < array2.Length; j++)
			{
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k] == null && !list2.Contains(k))
					{
						array[k] = array2[j];
						break;
					}
				}
			}
			return array;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public SearchResults SearchText(SearchSettings settings, string segment)
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
}
