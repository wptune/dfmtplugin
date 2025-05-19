using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LanguageMappingProvider.Database;
using LanguageMappingProvider.Database.Interface;
using LanguageMappingProvider.Model;
using MicrosoftTranslatorProvider.Commands;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Interfaces;
using MicrosoftTranslatorProvider.Model;

namespace MicrosoftTranslatorProvider.ViewModel;

public class LanguageMappingProviderViewModel : BaseModel
{
	private readonly ILanguageMappingDatabase _database;

	private readonly ITranslationOptions _translationOptions;

	private ObservableCollection<LanguageMapping> _filteredMappedLanguages;

	private ObservableCollection<LanguageMapping> _mappedLanguages;

	private LanguageMapping _selectedMappedLanguage;

	private bool _dialogResult;

	private bool _canResetToDefaults;

	private string _filter;

	private string _languagesCountMessage;

	private ICommand _applyChangesCommand;

	private ICommand _cancelChangesCommand;

	private ICommand _resetToDefaultCommand;

	private ICommand _clearCommand;

	public ObservableCollection<LanguageMapping> MappedLanguages
	{
		get
		{
			return _mappedLanguages;
		}
		set
		{
			if (_mappedLanguages != value)
			{
				_mappedLanguages = value;
				OnPropertyChanged("MappedLanguages");
			}
		}
	}

	public ObservableCollection<LanguageMapping> FilteredMappedLanguages
	{
		get
		{
			return _filteredMappedLanguages;
		}
		set
		{
			if (_filteredMappedLanguages != value)
			{
				_filteredMappedLanguages = value;
				OnPropertyChanged("FilteredMappedLanguages");
				RefreshLanguagesCountMessage();
			}
		}
	}

	public LanguageMapping SelectedMappedLanguage
	{
		get
		{
			return _selectedMappedLanguage;
		}
		set
		{
			if (_selectedMappedLanguage != value)
			{
				_selectedMappedLanguage = value;
				OnPropertyChanged("SelectedMappedLanguage");
			}
		}
	}

	public string Filter
	{
		get
		{
			return _filter;
		}
		set
		{
			if (!(_filter == value))
			{
				_filter = value?.ToLower();
				OnPropertyChanged("Filter");
				ApplyFilter();
				RefreshLanguagesCountMessage();
			}
		}
	}

	public string LanguagesCountMessage
	{
		get
		{
			return _languagesCountMessage;
		}
		set
		{
			if (!(_languagesCountMessage == value))
			{
				_languagesCountMessage = value;
				OnPropertyChanged("LanguagesCountMessage");
			}
		}
	}

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

	public bool CanResetToDefaults
	{
		get
		{
			return _canResetToDefaults;
		}
		set
		{
			if (_canResetToDefaults != value)
			{
				_canResetToDefaults = value;
				OnPropertyChanged("CanResetToDefaults");
			}
		}
	}

	public ICommand ApplyChangesCommand => _applyChangesCommand ?? (_applyChangesCommand = new RelayCommand(ApplyChanges, CanApplyChanges));

	public ICommand CancelChangesCommand => _cancelChangesCommand ?? (_cancelChangesCommand = new RelayCommand(CancelChanges));

	public ICommand ResetToDefaultCommand => _resetToDefaultCommand ?? (_resetToDefaultCommand = new RelayCommand(ResetToDefault));

	public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(Clear));

	public event MainWindowViewModel.CloseWindowEventRaiser CloseEventRaised;

	public LanguageMappingProviderViewModel(ITranslationOptions translationOptions, bool editProvider)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		_translationOptions = translationOptions;
		CanResetToDefaults = editProvider;
		_database = (ILanguageMappingDatabase)new LanguageMappingDatabase("microsoft", (IList<LanguageMapping>)(CanResetToDefaults ? DatabaseExtensions.GetDefaultMapping(_translationOptions) : null));
		RetrieveMappedLanguagesFromDatabase();
		FilteredMappedLanguages = MappedLanguages;
		base.PropertyChanged += FilterPropertyChangedHandler;
	}

	private void RetrieveMappedLanguagesFromDatabase()
	{
		IEnumerable<LanguageMapping> mappedLanguages = _database.GetMappedLanguages();
		IEnumerable<LanguageMapping> collection = mappedLanguages.Select((Func<LanguageMapping, LanguageMapping>)((LanguageMapping pair) => new LanguageMapping
		{
			Index = pair.Index,
			Name = pair.Name,
			Region = pair.Region,
			TradosCode = pair.TradosCode,
			LanguageCode = pair.LanguageCode
		}));
		MappedLanguages = new ObservableCollection<LanguageMapping>(collection);
		FilteredMappedLanguages = MappedLanguages;
		Filter = string.Empty;
	}

	private void RefreshLanguagesCountMessage()
	{
		int count = MappedLanguages.Count;
		int count2 = FilteredMappedLanguages.Count;
		LanguagesCountMessage = (string.IsNullOrWhiteSpace(Filter) ? $"Total languages: {count}" : $"Total languages: {count}; Filtered: {count2}");
	}

	private void ApplyFilter()
	{
		if (string.IsNullOrWhiteSpace(Filter))
		{
			FilteredMappedLanguages = new ObservableCollection<LanguageMapping>(MappedLanguages);
			return;
		}
		string filterLower = Filter.ToLower();
		IEnumerable<LanguageMapping> collection = MappedLanguages.Where((LanguageMapping language) => (!string.IsNullOrEmpty(language.Name) && language.Name.IndexOf(filterLower, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(language.Region) && language.Region.IndexOf(filterLower, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(language.TradosCode) && language.TradosCode.IndexOf(filterLower, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(language.LanguageCode) && language.LanguageCode.IndexOf(filterLower, StringComparison.OrdinalIgnoreCase) >= 0));
		FilteredMappedLanguages = new ObservableCollection<LanguageMapping>(collection);
	}

	private void ResetToDefault(object parameter)
	{
		if (ExecuteAction("Warning: Resetting to default values!\nAll changes will be lost and the database will be restored to its original state.\n\nThis action cannot be undone.", "Reset to default"))
		{
			_database.ResetToDefault();
			RetrieveMappedLanguagesFromDatabase();
		}
	}

	private bool ExecuteAction(string message, string title)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		MessageBoxResult val = MessageBox.Show(message, title, (MessageBoxButton)1, (MessageBoxImage)48);
		return (int)val == 1;
	}

	private void Clear(object parameter)
	{
		if (parameter is string text)
		{
			string text2 = text;
			string text3 = text2;
			if (text3 == "Filter")
			{
				Filter = string.Empty;
			}
		}
	}

	private void ApplyChanges(object parameter)
	{
		string filter = Filter;
		_database.UpdateAll((IEnumerable<LanguageMapping>)MappedLanguages);
		RetrieveMappedLanguagesFromDatabase();
		Filter = filter;
	}

	private bool CanApplyChanges(object parameter)
	{
		return _database.HasMappedLanguagesChanged((IEnumerable<LanguageMapping>)MappedLanguages);
	}

	private void CancelChanges(object parameter)
	{
		ShutDownApp();
	}

	private void ShutDownApp()
	{
		DialogResult = true;
		this.CloseEventRaised?.Invoke();
	}

	private void FilterPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Filter")
		{
			ApplyFilter();
		}
	}
}
