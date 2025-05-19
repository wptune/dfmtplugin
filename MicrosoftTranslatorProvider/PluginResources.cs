using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MicrosoftTranslatorProvider;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class PluginResources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("MicrosoftTranslatorProvider.PluginResources", typeof(PluginResources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ApiKeyError => ResourceManager.GetString("ApiKeyError", resourceCulture);

	public static string AppLogFolder => ResourceManager.GetString("AppLogFolder", resourceCulture);

	public static string BackBtnText => ResourceManager.GetString("BackBtnText", resourceCulture);

	public static Bitmap backImg
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("backImg", resourceCulture);
			return (Bitmap)@object;
		}
	}

	public static string BrowseBtn => ResourceManager.GetString("BrowseBtn", resourceCulture);

	public static string CatIdDescription => ResourceManager.GetString("CatIdDescription", resourceCulture);

	public static string CatIdError => ResourceManager.GetString("CatIdError", resourceCulture);

	public static Icon Download
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("Download", resourceCulture);
			return (Icon)@object;
		}
	}

	public static string EditSettingsErrorCaption => ResourceManager.GetString("EditSettingsErrorCaption", resourceCulture);

	public static string EditSettingsGenericErrorMessage => ResourceManager.GetString("EditSettingsGenericErrorMessage", resourceCulture);

	public static string EditSettingsXmlErrorMessage => ResourceManager.GetString("EditSettingsXmlErrorMessage", resourceCulture);

	public static string EmptyJsonFilePathMsg => ResourceManager.GetString("EmptyJsonFilePathMsg", resourceCulture);

	public static string EmptyProjectNameMsd => ResourceManager.GetString("EmptyProjectNameMsd", resourceCulture);

	public static Icon ForumIcon
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("ForumIcon", resourceCulture);
			return (Icon)@object;
		}
	}

	public static string InvalidProjectName => ResourceManager.GetString("InvalidProjectName", resourceCulture);

	public static string LangPairAuthErrorMsg1 => ResourceManager.GetString("LangPairAuthErrorMsg1", resourceCulture);

	public static string LangPairAuthErrorMsg2 => ResourceManager.GetString("LangPairAuthErrorMsg2", resourceCulture);

	public static string LangPairAuthErrorMsg3 => ResourceManager.GetString("LangPairAuthErrorMsg3", resourceCulture);

	public static string LogsFileName => ResourceManager.GetString("LogsFileName", resourceCulture);

	public static string LogsFolderPath => ResourceManager.GetString("LogsFolderPath", resourceCulture);

	public static string lookupFileStructureCheckErrorCaption => ResourceManager.GetString("lookupFileStructureCheckErrorCaption", resourceCulture);

	public static string lookupFileStructureCheckGenericErrorMessage => ResourceManager.GetString("lookupFileStructureCheckGenericErrorMessage", resourceCulture);

	public static string Microsoft => ResourceManager.GetString("Microsoft", resourceCulture);

	public static string Microsoft_Description => ResourceManager.GetString("Microsoft_Description", resourceCulture);

	public static Bitmap microsoft_image
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("microsoft_image", resourceCulture);
			return (Bitmap)@object;
		}
	}

	public static string Microsoft_Name => ResourceManager.GetString("Microsoft_Name", resourceCulture);

	public static string Microsoft_NiceName => ResourceManager.GetString("Microsoft_NiceName", resourceCulture);

	public static string Microsoft_ShortName => ResourceManager.GetString("Microsoft_ShortName", resourceCulture);

	public static string Microsoft_Tooltip => ResourceManager.GetString("Microsoft_Tooltip", resourceCulture);

	public static string MicrosoftApiDescription => ResourceManager.GetString("MicrosoftApiDescription", resourceCulture);

	public static string MicrosoftTranslatorProviderPlugin_WinFormsUI => ResourceManager.GetString("MicrosoftTranslatorProviderPlugin_WinFormsUI", resourceCulture);

	public static string MsApiBadCredentialsMessage => ResourceManager.GetString("MsApiBadCredentialsMessage", resourceCulture);

	public static string MsApiCategoryIdErrorMessage => ResourceManager.GetString("MsApiCategoryIdErrorMessage", resourceCulture);

	public static string MsApiFailedGetLanguagesMessage => ResourceManager.GetString("MsApiFailedGetLanguagesMessage", resourceCulture);

	public static string MsApiFailedToTranslateMessage => ResourceManager.GetString("MsApiFailedToTranslateMessage", resourceCulture);

	public static string MSSubscriptionRegionLabel => ResourceManager.GetString("MSSubscriptionRegionLabel", resourceCulture);

	public static Icon mstp_icon
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("mstp_icon", resourceCulture);
			return (Icon)@object;
		}
	}

	public static string MSTPCategoryTooltip => ResourceManager.GetString("MSTPCategoryTooltip", resourceCulture);

	public static string MSTPRegionTooltip => ResourceManager.GetString("MSTPRegionTooltip", resourceCulture);

	public static string PersistMicrosoft => ResourceManager.GetString("PersistMicrosoft", resourceCulture);

	public static string Plugin_Description => ResourceManager.GetString("Plugin_Description", resourceCulture);

	public static string Plugin_Name => ResourceManager.GetString("Plugin_Name", resourceCulture);

	public static string Plugin_NiceName => ResourceManager.GetString("Plugin_NiceName", resourceCulture);

	public static string Plugin_Tooltip => ResourceManager.GetString("Plugin_Tooltip", resourceCulture);

	public static string PluginsView => ResourceManager.GetString("PluginsView", resourceCulture);

	public static string PostLookupBrowse => ResourceManager.GetString("PostLookupBrowse", resourceCulture);

	public static string PostLookupDescription => ResourceManager.GetString("PostLookupDescription", resourceCulture);

	public static string PostLookupEmptyMessage => ResourceManager.GetString("PostLookupEmptyMessage", resourceCulture);

	public static string PostLookupFileName => ResourceManager.GetString("PostLookupFileName", resourceCulture);

	public static string PostLookupWaterMark => ResourceManager.GetString("PostLookupWaterMark", resourceCulture);

	public static string PostLookupWrongPathMessage => ResourceManager.GetString("PostLookupWrongPathMessage", resourceCulture);

	public static string PreLookBrowse => ResourceManager.GetString("PreLookBrowse", resourceCulture);

	public static string PreLookDescription => ResourceManager.GetString("PreLookDescription", resourceCulture);

	public static string PreLookFileName => ResourceManager.GetString("PreLookFileName", resourceCulture);

	public static string PreLookupEmptyMessage => ResourceManager.GetString("PreLookupEmptyMessage", resourceCulture);

	public static string PreLookupWaterMark => ResourceManager.GetString("PreLookupWaterMark", resourceCulture);

	public static string PreLookupWrongPathMessage => ResourceManager.GetString("PreLookupWrongPathMessage", resourceCulture);

	public static string ProjectLocationValidation => ResourceManager.GetString("ProjectLocationValidation", resourceCulture);

	public static string PromptForCredentialsCaption_Microsoft => ResourceManager.GetString("PromptForCredentialsCaption_Microsoft", resourceCulture);

	public static string ProviderControl_CategoryFieldName => ResourceManager.GetString("ProviderControl_CategoryFieldName", resourceCulture);

	public static string ProviderControl_CategoryLearnMore => ResourceManager.GetString("ProviderControl_CategoryLearnMore", resourceCulture);

	public static string ProviderControl_CategoryToolTip => ResourceManager.GetString("ProviderControl_CategoryToolTip", resourceCulture);

	public static string ProviderControl_RegionFieldName => ResourceManager.GetString("ProviderControl_RegionFieldName", resourceCulture);

	public static string ProviderControl_RegionLearnMore => ResourceManager.GetString("ProviderControl_RegionLearnMore", resourceCulture);

	public static string ProviderControl_RegionToolTip => ResourceManager.GetString("ProviderControl_RegionToolTip", resourceCulture);

	public static Icon Question
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("Question", resourceCulture);
			return (Icon)@object;
		}
	}

	public static string ReSendDescription => ResourceManager.GetString("ReSendDescription", resourceCulture);

	public static string SendPlainDescription => ResourceManager.GetString("SendPlainDescription", resourceCulture);

	public static Bitmap Setting
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("Setting", resourceCulture);
			return (Bitmap)@object;
		}
	}

	public static Icon Settings
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			object @object = ResourceManager.GetObject("Settings", resourceCulture);
			return (Icon)@object;
		}
	}

	public static string SettingsText => ResourceManager.GetString("SettingsText", resourceCulture);

	public static string SettingsView => ResourceManager.GetString("SettingsView", resourceCulture);

	public static string SettingsViewTitle => ResourceManager.GetString("SettingsViewTitle", resourceCulture);

	public static string TranslationLookupDraftNotResentMessage => ResourceManager.GetString("TranslationLookupDraftNotResentMessage", resourceCulture);

	public static string UnidirectionalGlossary => ResourceManager.GetString("UnidirectionalGlossary", resourceCulture);

	public static string UriNotSupportedMessage => ResourceManager.GetString("UriNotSupportedMessage", resourceCulture);

	public static string WindowDescription => ResourceManager.GetString("WindowDescription", resourceCulture);

	public static string WindowsControl_Close => ResourceManager.GetString("WindowsControl_Close", resourceCulture);

	public static string WrongJsonFilePath => ResourceManager.GetString("WrongJsonFilePath", resourceCulture);

	internal PluginResources()
	{
	}
}
