using Sdl.LanguagePlatform.Core;

namespace CustomMTTranslation;

internal class MtTag
{
	internal Tag SdlTag { get; }

	internal MtTag(Tag tag)
	{
		SdlTag = tag;
	}
}
