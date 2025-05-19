using System.Diagnostics;
using System.Drawing;
using Sdl.TellMe.ProviderApi;

namespace MicrosoftTranslatorProvider.TellMe;

public class CommunityForumAction : AbstractTellMeAction
{
	public override bool IsAvailable => true;

	public override string Category => "Microsoft Translator Provider";

	public override Icon Icon => PluginResources.ForumIcon;

	public CommunityForumAction()
	{
		((AbstractTellMeAction)this).Name = "Microsoft Translator Provider - Forum";
	}

	public override void Execute()
	{
		Process.Start("https://community.rws.com/product-groups/trados-portfolio/rws-appstore/");
	}
}
