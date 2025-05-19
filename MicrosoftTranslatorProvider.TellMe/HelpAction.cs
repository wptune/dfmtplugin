using System.Diagnostics;
using System.Drawing;
using Sdl.TellMe.ProviderApi;

namespace MicrosoftTranslatorProvider.TellMe;

public class HelpAction : AbstractTellMeAction
{
	public override bool IsAvailable => true;

	public override string Category => "Microsoft Translator Provider";

	public override Icon Icon => PluginResources.Question;

	public HelpAction()
	{
		((AbstractTellMeAction)this).Name = "Microsoft Translator Provider - Wiki";
	}

	public override void Execute()
	{
		Process.Start("https://community.rws.com/product-groups/trados-portfolio/rws-appstore/w/wiki/6546/microsoft-machine-translation-provider");
	}
}
