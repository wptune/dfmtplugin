using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using MicrosoftTranslatorProvider.Commands;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;
using Sdl.LanguagePlatform.Core;

namespace MicrosoftTranslatorProvider.ViewModel;

public class ProviderViewModel : BaseModel, IProviderViewModel
{
	private readonly LanguagePair[] _languagePairs;

	private readonly ITranslationOptions _options;

	private PairMapping _selectedLanguageMapping;

	private List<PairMapping> _languageMappings;

	private List<RegionSubscription> _regions;

	private RegionSubscription _selectedRegion;

	private bool _editProvider;

	private bool _persistMicrosoftKey;

	private string _apiKey;

	private ICommand _learnMoreCommand;

	public BaseModel ViewModel => this;

	public bool EditProvider
	{
		get
		{
			return _editProvider;
		}
		set
		{
			if (_editProvider != value)
			{
				_editProvider = value;
				OnPropertyChanged("EditProvider");
			}
		}
	}

	public string ApiKey
	{
		get
		{
			return _apiKey;
		}
		set
		{
			if (!(_apiKey == value))
			{
				_apiKey = value.Trim();
				OnPropertyChanged("ApiKey");
			}
		}
	}

	public RegionSubscription SelectedRegion
	{
		get
		{
			return _selectedRegion;
		}
		set
		{
			if (_selectedRegion != value)
			{
				_selectedRegion = value;
				OnPropertyChanged("SelectedRegion");
			}
		}
	}

	public List<RegionSubscription> Regions
	{
		get
		{
			return _regions ?? (_regions = new List<RegionSubscription>(new RegionsProvider().GetSubscriptionRegions()));
		}
		set
		{
			if (_regions != value)
			{
				_regions = value;
				OnPropertyChanged("Regions");
			}
		}
	}

	public bool PersistMicrosoftKey
	{
		get
		{
			return _persistMicrosoftKey;
		}
		set
		{
			if (_persistMicrosoftKey != value)
			{
				_persistMicrosoftKey = value;
				OnPropertyChanged("PersistMicrosoftKey");
			}
		}
	}

	public List<PairMapping> LanguageMappings
	{
		get
		{
			return _languageMappings;
		}
		set
		{
			if (_languageMappings != value)
			{
				_languageMappings = value;
				OnPropertyChanged("LanguageMappings");
			}
		}
	}

	public PairMapping SelectedLanguageMapping
	{
		get
		{
			return _selectedLanguageMapping;
		}
		set
		{
			if (_selectedLanguageMapping != value)
			{
				_selectedLanguageMapping = value;
				OnPropertyChanged("SelectedLanguageMapping");
			}
		}
	}

	public ICommand LearnMoreCommand => _learnMoreCommand ?? (_learnMoreCommand = new RelayCommand(delegate(object parameter)
	{
		Process.Start(parameter as string);
	}));

	public ProviderViewModel(ITranslationOptions options, LanguagePair[] languagePairs, bool editProvider)
	{
		_options = options;
		_languagePairs = languagePairs;
		EditProvider = editProvider;
		InitializeComponent();
		CreateMapping();
	}

	private void InitializeComponent()
	{
		ApiKey = _options.ApiKey;
		PersistMicrosoftKey = _options.PersistMicrosoftCredentials;
		SelectedRegion = Regions.FirstOrDefault((RegionSubscription a) => a.Key == _options.Region) ?? Regions.ElementAt(0);
	}

	private void CreateMapping()
	{
		if (_options.LanguageMappings != null)
		{
			LoadLanguageMappings();
			return;
		}
		List<PairMapping> list = new List<PairMapping>();
		LanguagePair[] languagePairs = _languagePairs;
		foreach (LanguagePair val in languagePairs)
		{
			CultureInfo cultureInfo = new CultureInfo(val.SourceCultureName);
			CultureInfo cultureInfo2 = new CultureInfo(val.TargetCultureName);
			list.Add(new PairMapping
			{
				DisplayName = cultureInfo.DisplayName + " - " + cultureInfo2.DisplayName,
				CategoryID = string.Empty,
				LanguagePair = val
			});
		}
		LanguageMappings = list;
	}

	private void LoadLanguageMappings()
	{
		List<PairMapping> list = new List<PairMapping>();
		foreach (PairMapping languageMapping in _options.LanguageMappings)
		{
			list.Add(new PairMapping
			{
				DisplayName = languageMapping.DisplayName,
				CategoryID = languageMapping.CategoryID,
				LanguagePair = languageMapping.LanguagePair
			});
		}
		LanguageMappings = list;
	}
}
