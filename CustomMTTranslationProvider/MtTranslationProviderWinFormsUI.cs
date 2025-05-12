using System;
using System.Drawing;
using System.Windows.Forms;
using CustomMTTranslation.Properties;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace CustomMTTranslationProvider;

[TranslationProviderWinFormsUi(Id = "MtTranslationProviderWinFormsUI", Name = "MtTranslationProviderWinFormsUI", Description = "MtTranslationProviderWinFormsUI")]
public class MtTranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
{
	public bool SupportsEditing => true;

	public string TypeDescription => Storage.Description;

	public string TypeName => Storage.TranslationProviderLabel;

	public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		MtProviderConfDialog mtProviderConfDialog = new MtProviderConfDialog(languagePairs[0]);
		if ((int)((Form)mtProviderConfDialog).ShowDialog(owner) == 1)
		{
			MtTranslationProvider mtTranslationProvider = new MtTranslationProvider(mtProviderConfDialog._Options);
			return (ITranslationProvider[])(object)new ITranslationProvider[1] { mtTranslationProvider };
		}
		return null;
	}

	public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		MtTranslationProvider mtTranslationProvider = translationProvider as MtTranslationProvider;
		string token = Storage.ExtractTokenFromUrl(translationProvider.Uri.AbsoluteUri);
		string model = Storage.ExtractModelFromUrl(translationProvider.Uri.AbsoluteUri);
		if (mtTranslationProvider == null)
		{
			return false;
		}
		MtProviderConfDialog mtProviderConfDialog = new MtProviderConfDialog(token, model, languagePairs[0]);
		if ((int)((Form)mtProviderConfDialog).ShowDialog(owner) == 1)
		{
			mtTranslationProvider.Options = mtProviderConfDialog._Options;
			translationProvider = (ITranslationProvider)(object)mtTranslationProvider;
			translationProvider.RefreshStatusInfo();
			return true;
		}
		return false;
	}

	public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
	{
		return true;
	}

	public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		return new TranslationProviderDisplayInfo
		{
			Name = Storage.TranslationProviderInfoNamePrefix + " <" + Storage.ExtractModelFromUrl(translationProviderUri.AbsoluteUri).Replace("+", " ") + ">",
			TooltipText = Storage.Description,
			TranslationProviderIcon = Resources.favicon,
			SearchResultImage = (Image)(object)Resources.logo
		};
	}

	public bool SupportsTranslationProviderUri(Uri translationProviderUri)
	{
		if (translationProviderUri == null)
		{
			throw new ArgumentNullException(Storage.UriNotSupportedException);
		}
		return string.Equals(translationProviderUri.Scheme, Storage.Urischeme, StringComparison.CurrentCultureIgnoreCase);
	}
}
