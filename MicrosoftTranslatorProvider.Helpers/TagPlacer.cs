using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MicrosoftTranslatorProvider.Model;
using NLog;
using Sdl.LanguagePlatform.Core;

namespace MicrosoftTranslatorProvider.Helpers;

public class TagPlacer
{
	private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	private readonly Segment _sourceSegment;

	private Dictionary<string, MicrosoftTag> _tagsDictionary;

	private string _returnedText;

	private MicrosoftTag _currentTag;

	public List<TagInfo> TagsInfo { get; set; } = new List<TagInfo>();


	public string PreparedSourceText { get; private set; }

	public TagPlacer(Segment sourceSegment)
	{
		_sourceSegment = sourceSegment;
		TagsInfo = new List<TagInfo>();
		GetSourceTagsDictionary();
	}

	public Segment GetTaggedSegment(string returnedText)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			return TryGetTaggedSegment(returnedText);
		}
		catch (Exception e)
		{
			LogException(e);
		}
		return new Segment();
	}

	private string AddSeparators(string text, MatchCollection matches)
	{
		foreach (Match match in matches)
		{
			text = text.Replace(match.Value, "```" + match.Value + "```");
		}
		return text;
	}

	public string MarkTags(string translation, string pattern)
	{
		try
		{
			MatchCollection matchCollection = new Regex(pattern).Matches(translation);
			if (matchCollection.Count > 0)
			{
				return AddSeparators(translation, matchCollection);
			}
		}
		catch (Exception e)
		{
			LogException(e);
		}
		return translation;
	}

	private Segment TryGetTaggedSegment(string returnedText)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		_returnedText = new HtmlUtil().HtmlDecode(returnedText);
		Segment val = new Segment();
		string[] targetElements = GetTargetElements();
		foreach (string text in targetElements)
		{
			if (_tagsDictionary.ContainsKey(text))
			{
				AddTagPadding(val, text);
			}
			else if (text.Trim().Length > 0)
			{
				val.Add(text.Trim());
			}
		}
		return val;
	}

	private string[] GetTargetElements()
	{
		string returnedText = _returnedText;
		returnedText = MarkTags(returnedText, "(<tg[0-9]*\\>)|(<\\/tg[0-9]*\\>)|(\\<tg[0-9]*/\\>)");
		returnedText = MarkTags(returnedText, "(<tg[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}/>)|(<\\\\/tg[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\\\\>)|(<tg[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}>)");
		returnedText = MarkTags(returnedText, "(<tgpt[0-9]*\\>)|(<\\/tgpt[0-9]*\\>)|(\\<tgpt[0-9]*/\\>)");
		returnedText = MarkTags(returnedText, "(<tg[0-9,\\.]*\\>)|(<\\/tg[0-9,\\.]*\\>)|(\\<tg[0-9,\\.]*/\\>)");
		return returnedText.Split(new string[1] { "```" }, StringSplitOptions.None);
	}

	private void AddTagPadding(Segment segment, string text)
	{
		string padLeft = _tagsDictionary[text].PadLeft;
		string padRight = _tagsDictionary[text].PadRight;
		string text2 = padLeft;
		if (text2.Length > 0)
		{
			segment.Add(text2);
		}
		segment.Add((SegmentElement)(object)_tagsDictionary[text].SdlTag);
		if (padRight.Length > 0)
		{
			segment.Add(padRight);
		}
	}

	private void GetSourceTagsDictionary()
	{
		try
		{
			TryGetSourceTagsDictionary();
			TagsInfo.Clear();
		}
		catch (Exception e)
		{
			LogException(e);
		}
	}

	private void TryGetSourceTagsDictionary()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		_tagsDictionary = new Dictionary<string, MicrosoftTag>();
		Segment sourceSegment = _sourceSegment;
		List<SegmentElement> list = ((sourceSegment != null) ? sourceSegment.Elements : null);
		if (list == null || !list.Any())
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (((object)list[i]).GetType() != typeof(Tag))
			{
				PreparedSourceText += ((object)list[i]).ToString();
				continue;
			}
			_currentTag = new MicrosoftTag((Tag)list[i].Duplicate());
			UpdateTagsInfo(i);
			string text = ConvertTagToString();
			PreparedSourceText += text;
			SetWhiteSpace(list, i);
			_tagsDictionary.Add(text, _currentTag);
		}
	}

	private string ConvertTagToString()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected I4, but got Unknown
		TagInfo correspondingTag = GetCorrespondingTag(_currentTag.SdlTag.TagID);
		if (correspondingTag == null)
		{
			return string.Empty;
		}
		TagType type = _currentTag.SdlTag.Type;
		if (1 == 0)
		{
		}
		string result = (type - 1) switch
		{
			0 => "<tg" + correspondingTag.TagId + ">", 
			1 => "</tg" + correspondingTag.TagId + ">", 
			2 => "<tg" + correspondingTag.TagId + "/>", 
			3 => "<tg" + correspondingTag.TagId + "/>", 
			4 => "<tg" + correspondingTag.TagId + "/>", 
			_ => string.Empty, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private TagInfo GetCorrespondingTag(string tagId)
	{
		return TagsInfo.FirstOrDefault((TagInfo t) => t.TagId.Equals(tagId));
	}

	private void SetWhiteSpace(List<SegmentElement> elements, int currentIndex)
	{
		SetTrailingWhitespaces(elements, currentIndex - 1);
		SetLeadingWhitespaces(elements, currentIndex + 1);
	}

	private void SetLeadingWhitespaces(List<SegmentElement> elements, int nextIndex)
	{
		if (nextIndex < elements.Count && !(((object)elements[nextIndex]).GetType() == typeof(Tag)))
		{
			string text = ((object)elements[nextIndex]).ToString();
			int length = text.Length - text.TrimStart(Array.Empty<char>()).Length;
			_currentTag.PadRight = text.Substring(0, length);
		}
	}

	private void SetTrailingWhitespaces(List<SegmentElement> elements, int previousIndex)
	{
		if (previousIndex >= 0 && !(((object)elements[previousIndex]).GetType() == typeof(Tag)))
		{
			string text = ((object)elements[previousIndex]).ToString();
			if (!text.Trim().Equals(""))
			{
				int num = text.Length - text.TrimEnd(Array.Empty<char>()).Length;
				_currentTag.PadLeft = text.Substring(text.Length - num);
			}
		}
	}

	private void UpdateTagsInfo(int index)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (!TagsInfo.Any((TagInfo n) => n.TagId.Equals(_currentTag.SdlTag.TagID)))
		{
			TagsInfo.Add(new TagInfo
			{
				Index = index,
				IsClosed = ((int)_currentTag.SdlTag.Type == 2),
				TagId = _currentTag.SdlTag.TagID,
				TagType = _currentTag.SdlTag.Type
			});
		}
	}

	private void LogException(Exception e)
	{
		_logger.Error(MethodBase.GetCurrentMethod().Name + " " + e.Message + "\n " + e.StackTrace);
	}
}
