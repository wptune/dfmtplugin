using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using MicrosoftTranslatorProvider.View;
using MicrosoftTranslatorProvider.ViewModel;
using Newtonsoft.Json;
using Sdl.Core.Globalization;
using Sdl.Desktop.IntegrationApi;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Sdl.ProjectAutomation.Core;
using Sdl.ProjectAutomation.FileBased;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.Internal;

namespace MicrosoftTranslatorProvider.Studio;

[TranslationProviderWinFormsUi(Id = "MicrosoftTranslatorProviderPlugin_WinFormsUI", Name = "MicrosoftTranslatorProviderPlugin_WinFormsUI", Description = "MicrosoftTranslatorProviderPlugin_WinFormsUI")]
public class ProviderWinFormsUI : ITranslationProviderWinFormsUI
{
	public string TypeDescription => PluginResources.Plugin_Description;

	public string TypeName => PluginResources.Plugin_NiceName;

	public bool SupportsEditing => true;

	public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
	{
		TranslationOptions translationOptions = new TranslationOptions();
		return (ITranslationProvider[])(object)((!ShowProviderWindow(languagePairs, credentialStore, translationOptions).DialogResult) ? null : new ITranslationProvider[1]
		{
			new Provider(translationOptions)
		});
	}

	public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
	{
		if (!(translationProvider is Provider provider))
		{
			return false;
		}
		MainWindowViewModel mainWindowViewModel = ShowProviderWindow(languagePairs, credentialStore, provider.Options, editProvider: true);
		return mainWindowViewModel.DialogResult;
	}

	private MainWindowViewModel ShowProviderWindow(LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore, ITranslationOptions loadOptions, bool editProvider = false)
	{
		MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(loadOptions, credentialStore, languagePairs, editProvider);
		MainWindow mainWindow2 = new MainWindow();
		((FrameworkElement)mainWindow2).DataContext = mainWindowViewModel;
		MainWindow mainWindow = mainWindow2;
		mainWindowViewModel.CloseEventRaised += delegate
		{
			((Window)mainWindow).Close();
		};
		((Window)mainWindow).ShowDialog();
		return mainWindowViewModel;
	}

	public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (translationProviderState == null)
		{
			return new TranslationProviderDisplayInfo
			{
				SearchResultImage = (Image)(object)PluginResources.microsoft_image,
				TranslationProviderIcon = PluginResources.mstp_icon,
				TooltipText = PluginResources.Microsoft_NiceName,
				Name = PluginResources.Microsoft_NiceName
			};
		}
		TranslationOptions translationOptions = JsonConvert.DeserializeObject<TranslationOptions>(translationProviderState);
		string text = translationOptions.CustomProviderName.SetProviderName(translationOptions.UseCustomProviderName);
		return new TranslationProviderDisplayInfo
		{
			SearchResultImage = (Image)(object)PluginResources.microsoft_image,
			TranslationProviderIcon = PluginResources.mstp_icon,
			TooltipText = text,
			Name = text
		};
	}

	public bool SupportsTranslationProviderUri(Uri translationProviderUri)
	{
		if ((object)translationProviderUri == null)
		{
			throw new ArgumentNullException(PluginResources.UriNotSupportedMessage);
		}
		return string.Equals(translationProviderUri.Scheme, "microsofttranslatorprovider", StringComparison.CurrentCultureIgnoreCase);
	}

	public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		List<LanguagePair> list = new List<LanguagePair>();
		ProjectsController controller = ((AbstractApplication)ApplicationHost<SdlTradosStudioApplication>.Application).GetController<ProjectsController>();
		object obj;
		if (controller == null)
		{
			obj = null;
		}
		else
		{
			FileBasedProject currentProject = controller.CurrentProject;
			obj = ((currentProject != null) ? currentProject.GetProjectInfo() : null);
		}
		ProjectInfo val = (ProjectInfo)obj;
		if (val != null)
		{
			Language[] targetLanguages = val.TargetLanguages;
			foreach (Language val2 in targetLanguages)
			{
				LanguagePair item = new LanguagePair(CultureCode.op_Implicit(val.SourceLanguage.CultureInfo), CultureCode.op_Implicit(val2.CultureInfo));
				list.Add(item);
			}
		}
		TranslationOptions loadOptions = new TranslationOptions();
		MainWindowViewModel mainWindowViewModel = ShowProviderWindow(list.ToArray(), credentialStore, loadOptions);
		return mainWindowViewModel.DialogResult;
	}
}
