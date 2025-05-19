using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Xml.Serialization;
using MicrosoftTranslatorProvider.Commands;
using MicrosoftTranslatorProvider.Helpers;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using NLog;

namespace MicrosoftTranslatorProvider.ViewModel;

public class SettingsViewModel : BaseModel, ISettingsViewModel
{
	private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	private readonly ITranslationOptions _options;

	private bool _reSendDraft;

	private bool _sendPlainText;

	private bool _doPreLookup;

	private bool _doPostLookup;

	private string _preLookupFileName;

	private string _postLookupFileName;

	private string _errorMessage;

	private bool _useCustomProviderName;

	private string _customProviderName;

	private ICommand _clearCommand;

	public BaseModel ViewModel => this;

	public ICommand ShowMainWindowCommand { get; set; }

	public ICommand BrowseCommand { get; set; }

	public ICommand ShowSettingsCommand { get; set; }

	public bool ReSendDraft
	{
		get
		{
			return _reSendDraft;
		}
		set
		{
			if (_reSendDraft != value)
			{
				_reSendDraft = value;
				ErrorMessage = string.Empty;
				OnPropertyChanged("ReSendDraft");
			}
		}
	}

	public bool SendPlainText
	{
		get
		{
			return _sendPlainText;
		}
		set
		{
			if (_sendPlainText != value)
			{
				_sendPlainText = value;
				ErrorMessage = string.Empty;
				OnPropertyChanged("SendPlainText");
			}
		}
	}

	public bool DoPreLookup
	{
		get
		{
			return _doPreLookup;
		}
		set
		{
			if (_doPreLookup != value)
			{
				_doPreLookup = value;
				if (!_doPreLookup)
				{
					PreLookupFileName = string.Empty;
				}
				ErrorMessage = string.Empty;
				OnPropertyChanged("DoPreLookup");
			}
		}
	}

	public bool DoPostLookup
	{
		get
		{
			return _doPostLookup;
		}
		set
		{
			if (_doPostLookup != value)
			{
				_doPostLookup = value;
				if (!_doPostLookup)
				{
					PostLookupFileName = string.Empty;
				}
				OnPropertyChanged("DoPostLookup");
			}
		}
	}

	public string PreLookupFileName
	{
		get
		{
			return _preLookupFileName;
		}
		set
		{
			if (!(_preLookupFileName == value))
			{
				_preLookupFileName = value;
				ErrorMessage = string.Empty;
				OnPropertyChanged("PreLookupFileName");
			}
		}
	}

	public string PostLookupFileName
	{
		get
		{
			return _postLookupFileName;
		}
		set
		{
			if (!(_postLookupFileName == value))
			{
				_postLookupFileName = value;
				ErrorMessage = string.Empty;
				OnPropertyChanged("PostLookupFileName");
			}
		}
	}

	public string ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			if (!(_errorMessage == value))
			{
				_errorMessage = value;
				OnPropertyChanged("ErrorMessage");
			}
		}
	}

	public bool UseCustomProviderName
	{
		get
		{
			return _useCustomProviderName;
		}
		set
		{
			if (_useCustomProviderName != value)
			{
				_useCustomProviderName = value;
				OnPropertyChanged("UseCustomProviderName");
			}
		}
	}

	public string CustomProviderName
	{
		get
		{
			return _customProviderName;
		}
		set
		{
			if (!(_customProviderName == value))
			{
				_customProviderName = value;
				OnPropertyChanged("CustomProviderName");
			}
		}
	}

	public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(Clear));

	public SettingsViewModel(ITranslationOptions options)
	{
		_options = options;
		BrowseCommand = new RelayCommand(Browse);
		SetSavedSettings();
	}

	private void SetSavedSettings()
	{
		ReSendDraft = _options.ResendDrafts;
		SendPlainText = _options.SendPlainTextOnly;
		DoPreLookup = _options.UsePreEdit;
		PreLookupFileName = _options.PreLookupFilename;
		DoPostLookup = _options.UsePostEdit;
		PostLookupFileName = _options.PostLookupFilename;
		CustomProviderName = _options.CustomProviderName;
		UseCustomProviderName = _options.UseCustomProviderName;
	}

	private void Clear(object parameter)
	{
		if (parameter is string text)
		{
			switch (text)
			{
			case "PreLookupFileName":
				PreLookupFileName = string.Empty;
				break;
			case "PostLookupFileName":
				PostLookupFileName = string.Empty;
				break;
			case "CustomProviderName":
				CustomProviderName = string.Empty;
				break;
			}
		}
	}

	private void Browse(object commandParameter)
	{
		ErrorMessage = string.Empty;
		if (string.IsNullOrEmpty(commandParameter.ToString()))
		{
			return;
		}
		string text = new OpenFileDialogService().ShowDialog("XML Files(*.xml) | *.xml");
		if (!string.IsNullOrEmpty(text))
		{
			if (commandParameter.Equals(PluginResources.PreLookBrowse))
			{
				PreLookupFileName = text;
				CheckIfIsValidLookupFile(PreLookupFileName);
			}
			else if (commandParameter.Equals(PluginResources.PostLookupBrowse))
			{
				PostLookupFileName = text;
				CheckIfIsValidLookupFile(PostLookupFileName);
			}
		}
	}

	private void CheckIfIsValidLookupFile(string filePath)
	{
		try
		{
			using StreamReader textReader = new StreamReader(filePath);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(EditCollection));
			EditCollection editCollection = (EditCollection)xmlSerializer.Deserialize(textReader);
		}
		catch (InvalidOperationException)
		{
			string fileName = Path.GetFileName(filePath);
			ErrorMessage = PluginResources.lookupFileStructureCheckErrorCaption + " " + fileName;
		}
		catch (Exception ex2)
		{
			_logger.Error($"{MethodBase.GetCurrentMethod().Name}: {ex2}");
			ErrorMessage = PluginResources.lookupFileStructureCheckGenericErrorMessage + " " + ex2.Message;
		}
	}
}
