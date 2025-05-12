using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Sdl.LanguagePlatform.Core;

namespace CustomMTTranslation;

internal class MtTranslationProviderTagPlacer
{
	private string _returnedText;

	private string _preparedSourceText;

	private readonly Segment _sourceSegment;

	private Dictionary<string, MtTag> dict;

	public List<TagInfo> TagsInfo { get; set; }

	public string PreparedSourceText => _preparedSourceText;

	public MtTranslationProviderTagPlacer(Segment sourceSegment)
	{
		_sourceSegment = sourceSegment;
		TagsInfo = new List<TagInfo>();
		dict = GetSourceTagsDict();
	}

	private string DecodeReturnedText(string strInput)
	{
		strInput = HttpUtility.HtmlDecode(strInput);
		return strInput;
	}

	private Dictionary<string, MtTag> GetSourceTagsDict()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Invalid comparison between Unknown and I4
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Invalid comparison between Unknown and I4
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Invalid comparison between Unknown and I4
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Invalid comparison between Unknown and I4
		dict = new Dictionary<string, MtTag>();
		for (int i = 0; i < _sourceSegment.Elements.Count; i++)
		{
			if (((object)_sourceSegment.Elements[i]).GetType().ToString() == "Sdl.LanguagePlatform.Core.Tag")
			{
				MtTag mtTag = new MtTag((Tag)_sourceSegment.Elements[i].Duplicate());
				string text = string.Empty;
				TagInfo tagInfo = new TagInfo
				{
					TagType = mtTag.SdlTag.Type,
					Index = i,
					IsClosed = false,
					TagId = mtTag.SdlTag.TagID
				};
				if (!TagsInfo.Any((TagInfo n) => n.TagId.Equals(tagInfo.TagId)))
				{
					TagsInfo.Add(tagInfo);
				}
				TagInfo correspondingTag = GetCorrespondingTag(mtTag.SdlTag.TagID);
				if ((int)mtTag.SdlTag.Type == 1 && correspondingTag != null)
				{
					text = "<tg" + correspondingTag.TagId + ">";
				}
				if ((int)mtTag.SdlTag.Type == 2 && correspondingTag != null)
				{
					correspondingTag.IsClosed = true;
					text = "</tg" + correspondingTag.TagId + ">";
				}
				if (((int)mtTag.SdlTag.Type == 3 || (int)mtTag.SdlTag.Type == 4 || (int)mtTag.SdlTag.Type == 5) && correspondingTag != null)
				{
					text = "<tg" + correspondingTag.TagId + "/>";
				}
				_preparedSourceText += text;
				dict.Add(text, mtTag);
			}
			else
			{
				HttpUtility.HtmlEncode(((object)_sourceSegment.Elements[i]).ToString());
				_preparedSourceText += ((object)_sourceSegment.Elements[i]).ToString();
			}
		}
		TagsInfo.Clear();
		return dict;
	}

	private TagInfo GetCorrespondingTag(string tagId)
	{
		return TagsInfo.FirstOrDefault((TagInfo t) => t.TagId.Equals(tagId));
	}

	public Segment GetTaggedSegment(string returnedText)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		_returnedText = DecodeReturnedText(returnedText);
		Segment val = new Segment();
		string[] targetElements = GetTargetElements();
		for (int i = 0; i < targetElements.Length; i++)
		{
			string text = targetElements[i];
			if (dict.ContainsKey(text))
			{
				try
				{
					if (targetElements[i].Length > 0 && char.IsWhiteSpace(targetElements[i][0]))
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < targetElements[i].Length && char.IsWhiteSpace(targetElements[i][j]); j++)
						{
							stringBuilder.Append(targetElements[i][j]);
						}
						val.Add(stringBuilder.ToString());
					}
					val.Add((SegmentElement)(object)dict[text].SdlTag);
					if (targetElements[i].Length > 0 && char.IsWhiteSpace(targetElements[i][targetElements[i].Length - 1]))
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						int num = targetElements[i].Length - 1;
						while (num >= 0 && char.IsWhiteSpace(targetElements[i][num]))
						{
							stringBuilder2.Append(targetElements[i][num]);
							num--;
						}
						val.Add(stringBuilder2.ToString());
					}
				}
				catch
				{
				}
			}
			else
			{
				val.Add(text);
			}
		}
		return val;
	}

	public Segment RemoveTrailingClosingTags(Segment segment)
	{
		SegmentElement val = segment.Elements[segment.Elements.Count - 1];
		string text = ((object)val).ToString();
		Regex regex = new Regex("\\</tg[0-9a-z]*\\>");
		Type type = ((object)val).GetType();
		MatchCollection matchCollection = regex.Matches(text);
		if (type.ToString().Equals("Sdl.LanguagePlatform.Core.Text") && matchCollection.Count > 0)
		{
			foreach (Match item in matchCollection)
			{
				text = text.Replace(item.Value, "");
			}
			segment.Elements.Remove(val);
			segment.Add(text.TrimStart(Array.Empty<char>()));
		}
		return segment;
	}

	private string[] GetTargetElements()
	{
		string text = _returnedText;
		foreach (Match item in new Regex("(<tg[0-9a-z]*\\>)|(<\\/tg[0-9a-z]*\\>)|(\\<tg[0-9a-z]*/\\>)").Matches(_returnedText))
		{
			text = text.Replace(item.Value, "```" + item.Value + "```");
		}
		string[] separator = new string[1] { "```" };
		return text.Split(separator, StringSplitOptions.None);
	}
}
