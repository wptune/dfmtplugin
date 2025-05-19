using Sdl.TellMe.ProviderApi;

namespace MicrosoftTranslatorProvider.TellMe;

[TellMeProvider]
public class TellMeProvider : ITellMeProvider
{
	public string Name => "Microsoft Translator Provider - TellMe";

	public AbstractTellMeAction[] ProviderActions
	{
		get
		{
			AbstractTellMeAction[] array = new AbstractTellMeAction[3];
			StoreAction storeAction = new StoreAction();
			((AbstractTellMeAction)storeAction).Keywords = new string[7] { "mtp", "microsoft", "translator", "provider", "store", "update", "download" };
			array[0] = storeAction;
			CommunityForumAction communityForumAction = new CommunityForumAction();
			((AbstractTellMeAction)communityForumAction).Keywords = new string[8] { "mtp", "microsoft", "translator", "provider", "forum", "report", "community", "support" };
			array[1] = communityForumAction;
			HelpAction helpAction = new HelpAction();
			((AbstractTellMeAction)helpAction).Keywords = new string[7] { "mtp", "microsoft", "translator", "provider", "help", "guide", "wiki" };
			array[2] = helpAction;
			return (AbstractTellMeAction[])(object)array;
		}
	}
}
