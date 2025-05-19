using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MicrosoftTranslatorProvider.Extensions;

public static class StringExtensions
{
	public static string[] SplitAt(this string source, params int[] index)
	{
		index = (from x in index.Distinct()
			orderby x
			select x).ToArray();
		string[] array = new string[index.Length + 1];
		int num = 0;
		int num2 = 0;
		while (num2 < index.Length)
		{
			array[num2] = source.Substring(num, index[num2] - num);
			num = index[num2++];
		}
		array[index.Length] = source.Substring(num);
		return array;
	}

	public static string ReplaceCharacters(this string textToTranslate, MatchCollection matches)
	{
		List<int> list = new List<int>();
		foreach (Match match in matches)
		{
			if (match.Index.Equals(0))
			{
				list.Add(match.Length);
				continue;
			}
			list.Add(match.Index);
			string value = textToTranslate.Substring(match.Index + match.Length);
			if (!string.IsNullOrEmpty(value))
			{
				list.Add(match.Index + match.Length);
			}
		}
		List<string> list2 = textToTranslate.SplitAt(list.ToArray()).ToList();
		List<int> list3 = new List<int>();
		for (int i = 0; i < list2.Count; i++)
		{
			if (!list2[i].Contains("tg"))
			{
				list3.Add(i);
			}
		}
		foreach (int item in list3)
		{
			string input = list2[item];
			string input2 = Regex.Replace(input, "<", "&lt;");
			string value2 = Regex.Replace(input2, ">", "&gt;");
			list2[item] = value2;
		}
		return list2.Aggregate(string.Empty, (string current, string text) => current + text);
	}

	public static string SetProviderName(this string customName, bool useCustomName)
	{
		string microsoft_NiceName = PluginResources.Microsoft_NiceName;
		string text = string.Empty;
		if (!string.IsNullOrEmpty(customName) && useCustomName)
		{
			text = " - " + customName;
		}
		return microsoft_NiceName + text;
	}
}
