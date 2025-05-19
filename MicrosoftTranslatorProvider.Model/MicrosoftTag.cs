using Sdl.LanguagePlatform.Core;

namespace MicrosoftTranslatorProvider.Model;

public class MicrosoftTag
{
	public string PadLeft { get; set; } = string.Empty;


	public string PadRight { get; set; } = string.Empty;


	public Tag SdlTag { get; }

	public MicrosoftTag(Tag sdlTag)
	{
		SdlTag = sdlTag;
	}
}
