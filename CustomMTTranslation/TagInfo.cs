using Sdl.LanguagePlatform.Core;

namespace CustomMTTranslation;

public class TagInfo
{
	public string TagId { get; set; }

	public int Index { get; set; }

	public TagType TagType { get; set; }

	public bool IsClosed { get; set; }
}
