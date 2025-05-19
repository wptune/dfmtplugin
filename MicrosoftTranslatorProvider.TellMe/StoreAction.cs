using System.Diagnostics;
using System.Drawing;
using Sdl.TellMe.ProviderApi;

namespace MicrosoftTranslatorProvider.TellMe;

public class StoreAction : AbstractTellMeAction
{
	public override bool IsAvailable => true;

	public override string Category => "Microsoft Translator Provider";

	public override Icon Icon => PluginResources.Download;

	public StoreAction()
	{
		((AbstractTellMeAction)this).Name = "Microsoft Translator Provider - AppStore";
	}

	public override void Execute()
	{
		Process.Start("https://appstore.rws.com/plugin/179/");
	}
}
