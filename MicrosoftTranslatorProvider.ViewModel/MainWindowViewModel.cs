using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Input;
using MicrosoftTranslatorProvider.Commands;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Helpers;
using MicrosoftTranslatorProvider.Interface;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using MicrosoftTranslatorProvider.Studio.TranslationProvider;
using MicrosoftTranslatorProvider.View;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace MicrosoftTranslatorProvider.ViewModel;

public class MainWindowViewModel : BaseModel, IMainWindow
{
	public delegate void CloseWindowEventRaiser();

	private const string ViewDetails_Provider = "ProviderViewModel";

	private const string ViewDetails_Settings = "SettingsViewModel";

	private const string ViewDetails_PrivateEndpoint = "PrivateEndpointViewModel";

	private readonly ISettingsViewModel _settingsControlViewModel;

	private readonly IProviderViewModel _providerControlViewModel;

	private readonly IPrivateEndpointViewModel _privateEndpointViewModel;

	private readonly ITranslationProviderCredentialStore _credentialStore;

	private readonly LanguagePair[] _languagePairs;

	private readonly bool _editProvider;

	private List<string> _endpoints;

	private string _selectedEndpoint;

	private bool _usePrivateEndpoint;

	private bool _dialogResult;

	private bool _canSwitchProvider;

	private ViewDetails _selectedView;

	private string _multiButtonContent;

	private bool _canAccessLanguageMappingProvider;

	private ICommand _saveCommand;

	private ICommand _navigateToCommand;

	private ICommand _switchViewCommand;

	private ICommand _openLanguageMappingProviderCommand;

	public bool DialogResult
	{
		get
		{
			return _dialogResult;
		}
		set
		{
			if (_dialogResult != value)
			{
				_dialogResult = value;
				OnPropertyChanged("DialogResult");
			}
		}
	}

	public string MultiButtonContent
	{
		get
		{
			return _multiButtonContent;
		}
		set
		{
			if (!(_multiButtonContent == value))
			{
				_multiButtonContent = value;
				OnPropertyChanged("MultiButtonContent");
			}
		}
	}

	public bool CanAccessLanguageMappingProvider
	{
		get
		{
			return _canAccessLanguageMappingProvider;
		}
		set
		{
			if (_canAccessLanguageMappingProvider != value)
			{
				_canAccessLanguageMappingProvider = value;
				OnPropertyChanged("CanAccessLanguageMappingProvider");
			}
		}
	}

	public ViewDetails SelectedView
	{
		get
		{
			return _selectedView;
		}
		set
		{
			_selectedView = value;
			OnPropertyChanged("SelectedView");
		}
	}

	public bool UsePrivateEndpoint
	{
		get
		{
			return _usePrivateEndpoint;
		}
		set
		{
			if (_usePrivateEndpoint != value)
			{
				_usePrivateEndpoint = value;
				OnPropertyChanged("UsePrivateEndpoint");
			}
		}
	}

	public List<string> Endpoints
	{
		get
		{
			return _endpoints;
		}
		set
		{
			if (_endpoints != value)
			{
				_endpoints = value;
				OnPropertyChanged("Endpoints");
			}
		}
	}

	public string SelectedEndpoint
	{
		get
		{
			return _selectedEndpoint;
		}
		set
		{
			if (!(_selectedEndpoint == value))
			{
				_selectedEndpoint = value;
				UsePrivateEndpoint = _selectedEndpoint.Equals("Private Endpoint");
				SwitchView(_selectedEndpoint.Equals("Microsoft") ? "ProviderViewModel" : "PrivateEndpointViewModel");
				OnPropertyChanged("SelectedEndpoint");
			}
		}
	}

	public bool CanSwitchProvider
	{
		get
		{
			return _canSwitchProvider;
		}
		set
		{
			if (value != _canSwitchProvider)
			{
				_canSwitchProvider = value;
				OnPropertyChanged("CanSwitchProvider");
			}
		}
	}

	public List<ViewDetails> AvailableViews { get; set; }

	public ITranslationOptions TranslationOptions { get; set; }

	public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(Save));

	public ICommand NavigateToCommand => _navigateToCommand ?? (_navigateToCommand = new RelayCommand(NavigateTo));

	public ICommand SwitchViewCommand => _switchViewCommand ?? (_switchViewCommand = new RelayCommand(SwitchView));

	public ICommand OpenLanguageMappingProviderCommand => _openLanguageMappingProviderCommand ?? (_openLanguageMappingProviderCommand = new RelayCommand(OpenLanguageMappingProvider));

	public event CloseWindowEventRaiser CloseEventRaised;

	public MainWindowViewModel(ITranslationOptions options, ITranslationProviderCredentialStore credentialStore, LanguagePair[] languagePairs, bool editProvider = false)
	{
		TranslationOptions = options;
		CanAccessLanguageMappingProvider = File.Exists(Constants.DatabaseFilePath);
		_providerControlViewModel = new ProviderViewModel(options, languagePairs, editProvider);
		_settingsControlViewModel = new SettingsViewModel(options);
		_privateEndpointViewModel = new PrivateEndpointViewModel();
		_credentialStore = credentialStore;
		_languagePairs = languagePairs;
		_editProvider = editProvider;
		AvailableViews = new List<ViewDetails>
		{
			new ViewDetails
			{
				Name = "ProviderViewModel",
				ViewModel = _providerControlViewModel.ViewModel
			},
			new ViewDetails
			{
				Name = "SettingsViewModel",
				ViewModel = _settingsControlViewModel.ViewModel
			},
			new ViewDetails
			{
				Name = "PrivateEndpointViewModel",
				ViewModel = _privateEndpointViewModel.ViewModel
			}
		};
		Endpoints = new List<string> { "Microsoft", "Private Endpoint" };
		SelectedEndpoint = Endpoints.First();
		SwitchView(TranslationOptions.UsePrivateEndpoint ? "PrivateEndpointViewModel" : "ProviderViewModel");
		SetCredentialsOnUI();
		if (_editProvider)
		{
			DatabaseExtensions.CreateDatabase(TranslationOptions);
			CanAccessLanguageMappingProvider = File.Exists(Constants.DatabaseFilePath);
		}
	}

	public bool IsWindowValid()
	{
		bool flag = ValidSettingsPageOptions();
		return (!UsePrivateEndpoint) ? (flag && ValidMicrosoftOptions()) : (flag && ValidPrivateEndpointOptions());
	}

	private bool ValidPrivateEndpointOptions()
	{
		return !string.IsNullOrEmpty(_privateEndpointViewModel.Endpoint);
	}

	private bool ValidSettingsPageOptions()
	{
		if (_settingsControlViewModel.DoPreLookup && string.IsNullOrEmpty(_settingsControlViewModel.PreLookupFileName))
		{
			ErrorHandler.HandleError(PluginResources.PreLookupEmptyMessage, "Pre-lookup");
			return false;
		}
		if (_settingsControlViewModel.DoPreLookup && !File.Exists(_settingsControlViewModel.PreLookupFileName))
		{
			ErrorHandler.HandleError(PluginResources.PreLookupWrongPathMessage, "Pre-lookup");
			return false;
		}
		if (_settingsControlViewModel.DoPostLookup && string.IsNullOrEmpty(_settingsControlViewModel.PostLookupFileName))
		{
			ErrorHandler.HandleError(PluginResources.PostLookupEmptyMessage, "Post-lookup");
			return false;
		}
		if (_settingsControlViewModel.DoPostLookup && !File.Exists(_settingsControlViewModel.PostLookupFileName))
		{
			ErrorHandler.HandleError(PluginResources.PostLookupWrongPathMessage, "Post-lookup");
			return false;
		}
		return true;
	}

	private bool ValidMicrosoftOptions()
	{
		if (string.IsNullOrEmpty(_providerControlViewModel.ApiKey))
		{
			ErrorHandler.HandleError(PluginResources.ApiKeyError, "API Key");
			return false;
		}
		return AreMicrosoftCredentialsValid();
	}

	private void Save(object window)
	{
		if (IsWindowValid())
		{
			SetMicrosoftProviderOptions();
			SetGeneralProviderOptions();
			SaveCredentials();
			DatabaseExtensions.CreateDatabase(TranslationOptions);
			DialogResult = true;
			this.CloseEventRaised?.Invoke();
		}
	}

	private bool AreMicrosoftCredentialsValid()
	{
		try
		{
			if (TranslationOptions.UsePrivateEndpoint)
			{
				return true;
			}
			MicrosoftApi microsoftApi = new MicrosoftApi(_providerControlViewModel.ApiKey, _providerControlViewModel.SelectedRegion?.Key);
			return true;
		}
		catch (Exception innerException)
		{
			Exception exception = innerException;
			do
			{
				if (innerException.Message.Contains("remote name could not be resolved"))
				{
					ErrorHandler.HandleError("Couldn't connect on the selected region, please try again using the region that is associated with your account.", "Connection failed");
					return false;
				}
				if (innerException.Message.Contains("401 (Access Denied)"))
				{
					ErrorHandler.HandleError("Couldn't connect with the current configuration, please check your API Key and Region and try again.", "Connection failed");
					return false;
				}
				innerException = innerException.InnerException;
			}
			while (innerException != null);
			ErrorHandler.HandleError(exception);
			return false;
		}
	}

	private void SetGeneralProviderOptions()
	{
		if (_settingsControlViewModel != null)
		{
			TranslationOptions.SendPlainTextOnly = _settingsControlViewModel.SendPlainText;
			TranslationOptions.ResendDrafts = _settingsControlViewModel.ReSendDraft;
			TranslationOptions.UsePreEdit = _settingsControlViewModel.DoPreLookup;
			TranslationOptions.PreLookupFilename = _settingsControlViewModel.PreLookupFileName;
			TranslationOptions.UsePostEdit = _settingsControlViewModel.DoPostLookup;
			TranslationOptions.PostLookupFilename = _settingsControlViewModel.PostLookupFileName;
			TranslationOptions.CustomProviderName = _settingsControlViewModel.CustomProviderName;
			TranslationOptions.UseCustomProviderName = _settingsControlViewModel.UseCustomProviderName;
		}
		if (TranslationOptions != null && TranslationOptions.LanguagesSupported == null)
		{
			TranslationOptions.LanguagesSupported = new List<string>();
		}
		if (_languagePairs == null)
		{
			return;
		}
		LanguagePair[] languagePairs = _languagePairs;
		foreach (LanguagePair val in languagePairs)
		{
			if (!TranslationOptions.LanguagesSupported.Contains(val.TargetCultureName))
			{
				TranslationOptions?.LanguagesSupported?.Add(val.TargetCultureName);
			}
		}
	}

	private void SetMicrosoftProviderOptions()
	{
		TranslationOptions.ApiKey = _providerControlViewModel.ApiKey;
		TranslationOptions.Region = _providerControlViewModel.SelectedRegion.Key;
		TranslationOptions.PersistMicrosoftCredentials = _providerControlViewModel.PersistMicrosoftKey;
		TranslationOptions.LanguageMappings = _providerControlViewModel.LanguageMappings;
		TranslationOptions.UsePrivateEndpoint = UsePrivateEndpoint;
		TranslationOptions.PrivateEndpoint = _privateEndpointViewModel.Endpoint;
		TranslationOptions.Parameters = _privateEndpointViewModel.Parameters.ToList();
	}

	private void NavigateTo(object parameter)
	{
		Process.Start(parameter as string);
	}

	private void SwitchView(object parameter)
	{
		try
		{
			string requestedType;
			if (parameter is string text)
			{
				requestedType = text;
			}
			else
			{
				requestedType = ((SelectedView.Name == "ProviderViewModel" || SelectedView.Name == "PrivateEndpointViewModel") ? "SettingsViewModel" : ((SelectedEndpoint == "Microsoft") ? "ProviderViewModel" : "PrivateEndpointViewModel"));
			}
			MultiButtonContent = ((requestedType == "ProviderViewModel" || requestedType == "PrivateEndpointViewModel") ? "Settings" : "Provider");
			SelectedView = AvailableViews.FirstOrDefault((ViewDetails x) => x.Name == requestedType);
			CanSwitchProvider = _editProvider || SelectedView.Name == "SettingsViewModel";
		}
		catch
		{
		}
	}

	private void SetCredentialsOnUI()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		try
		{
			TranslationProviderUriBuilder val = new TranslationProviderUriBuilder("microsofttranslatorprovider");
			GenericCredentials val2 = new GenericCredentials(_credentialStore.GetCredential(val.Uri).Credential);
			if (val2 == null)
			{
				return;
			}
			bool.TryParse(val2["Persist-ApiKey"], out var result);
			_providerControlViewModel.PersistMicrosoftKey = result;
			_providerControlViewModel.ApiKey = ((_editProvider || result) ? val2["API-Key"] : string.Empty);
			_privateEndpointViewModel.Endpoint = val2["Endpoint"];
			IEnumerable<string> enumerable = from x in val2.ToCredentialString().Split(new char[1] { ';' })
				where x.StartsWith("header_")
				select x;
			foreach (string item in enumerable)
			{
				string[] array = item.Split(new char[1] { '=' });
				_privateEndpointViewModel.Headers.Add(new UrlMetadata
				{
					Key = HttpUtility.UrlDecode(array[0].Replace("header_", string.Empty)),
					Value = HttpUtility.UrlDecode(array[1].Replace("header_", string.Empty))
				});
			}
			IEnumerable<string> enumerable2 = from x in val2.ToCredentialString().Split(new char[1] { ';' })
				where x.StartsWith("parameter_")
				select x;
			foreach (string item2 in enumerable2)
			{
				string[] array2 = item2.Split(new char[1] { '=' });
				string text = array2[0].Replace("parameter_", string.Empty);
				string str = array2[1];
				if (!text.StartsWith("from") && !text.StartsWith("to"))
				{
					_privateEndpointViewModel.Parameters.Add(new UrlMetadata
					{
						Key = HttpUtility.UrlDecode(text),
						Value = HttpUtility.UrlDecode(str)
					});
				}
			}
			_privateEndpointViewModel.Headers = new ObservableCollection<UrlMetadata>(_privateEndpointViewModel.Headers.Where((UrlMetadata x) => x.Key != null && x.Value != null));
			_privateEndpointViewModel.Parameters = new ObservableCollection<UrlMetadata>(_privateEndpointViewModel.Parameters.Where((UrlMetadata x) => x.Key != null && x.Value != null));
		}
		catch
		{
		}
	}

	private void SaveCredentials()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		TranslationProviderUriBuilder val = new TranslationProviderUriBuilder("microsofttranslatorprovider");
		_credentialStore.RemoveCredential(val.Uri);
		bool persistMicrosoftKey = _providerControlViewModel.PersistMicrosoftKey;
		GenericCredentials val2 = new GenericCredentials("mstpusername", "mstppassword")
		{
			["Persist-ApiKey"] = persistMicrosoftKey.ToString(),
			["API-Key"] = _providerControlViewModel.ApiKey,
			["Endpoint"] = _privateEndpointViewModel.Endpoint
		};
		foreach (UrlMetadata item in _privateEndpointViewModel?.Headers)
		{
			if (item.Key != null && item.Value != null)
			{
				val2["header_" + item.Key] = item.Value;
			}
		}
		foreach (UrlMetadata item2 in _privateEndpointViewModel?.Parameters)
		{
			if (item2.Key != null && item2.Value != null)
			{
				val2["parameter_" + item2.Key] = item2.Value;
			}
		}
		TranslationProviderCredential val3 = new TranslationProviderCredential(((object)val2).ToString(), true);
		_credentialStore.AddCredential(val.Uri, val3);
	}

	private void OpenLanguageMappingProvider(object parameter)
	{
		LanguageMappingProviderViewModel languageMappingProviderViewModel = new LanguageMappingProviderViewModel(TranslationOptions, _editProvider);
		LanguageMappingProviderView languageMappingProviderView = new LanguageMappingProviderView();
		((FrameworkElement)languageMappingProviderView).DataContext = languageMappingProviderViewModel;
		LanguageMappingProviderView lmpView = languageMappingProviderView;
		languageMappingProviderViewModel.CloseEventRaised += delegate
		{
			((Window)lmpView).Close();
		};
		bool? flag = ((Window)lmpView).ShowDialog();
	}
}
