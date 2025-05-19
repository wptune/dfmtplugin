using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MicrosoftTranslatorProvider.Commands;
using MicrosoftTranslatorProvider.Interface;
using MicrosoftTranslatorProvider.Model;

namespace MicrosoftTranslatorProvider.ViewModel;

public class PrivateEndpointViewModel : BaseModel, IPrivateEndpointViewModel
{
	private string _endpoint;

	private ObservableCollection<UrlMetadata> _headers;

	private ObservableCollection<UrlMetadata> _parameters;

	private ICommand _clearCommand;

	private ICommand _addHeaderCommand;

	private ICommand _addParameterCommand;

	private ICommand _deletePairCommand;

	private ICommand _selectedItemChangedCommand;

	public BaseModel ViewModel => this;

	public string Endpoint
	{
		get
		{
			return _endpoint;
		}
		set
		{
			_endpoint = value;
			OnPropertyChanged("Endpoint");
		}
	}

	public ObservableCollection<UrlMetadata> Headers
	{
		get
		{
			return _headers;
		}
		set
		{
			_headers = value;
			OnPropertyChanged("Headers");
		}
	}

	public ObservableCollection<UrlMetadata> Parameters
	{
		get
		{
			return _parameters;
		}
		set
		{
			_parameters = value;
			OnPropertyChanged("Parameters");
		}
	}

	public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(Clear));

	public ICommand AddHeaderCommand => _addHeaderCommand ?? (_addHeaderCommand = new RelayCommand(AddHeader));

	public ICommand AddParameterCommand => _addParameterCommand ?? (_addParameterCommand = new RelayCommand(AddParameter));

	public ICommand DeletePairCommand => _deletePairCommand ?? (_deletePairCommand = new RelayCommand(DeletePair));

	public ICommand SelectedItemChangedCommand => _selectedItemChangedCommand ?? (_selectedItemChangedCommand = new RelayCommand(SelectedItemChanged));

	public PrivateEndpointViewModel()
	{
		Headers = new ObservableCollection<UrlMetadata>();
		Parameters = new ObservableCollection<UrlMetadata>();
		Endpoint = string.Empty;
		AddHeaderCommand.Execute(null);
		AddParameterCommand.Execute(new UrlMetadata
		{
			Key = "from",
			Value = "sourceLanguage",
			IsReadOnly = true
		});
		AddParameterCommand.Execute(new UrlMetadata
		{
			Key = "to",
			Value = "targetLanguage",
			IsReadOnly = true
		});
	}

	private void Clear(object parameter)
	{
		if (parameter is string text)
		{
			string text2 = text;
			string text3 = text2;
			if (text3 == "url")
			{
				Endpoint = string.Empty;
			}
		}
	}

	private void DeletePair(object parameter)
	{
		if (parameter is UrlMetadata item)
		{
			Headers.Remove(item);
			Parameters.Remove(item);
		}
	}

	private void AddHeader(object parameter)
	{
		Headers.Add((parameter is UrlMetadata urlMetadata) ? urlMetadata : new UrlMetadata());
	}

	private void AddParameter(object parameter)
	{
		Parameters.Add((parameter is UrlMetadata urlMetadata) ? urlMetadata : new UrlMetadata());
	}

	private void SelectedItemChanged(object parameter)
	{
		if (parameter is UrlMetadata urlMetadata)
		{
			UrlMetadata urlMetadata2 = Headers.FirstOrDefault((UrlMetadata x) => x.IsSelected);
			if (urlMetadata2 != null)
			{
				urlMetadata2.IsSelected = false;
			}
			urlMetadata.IsSelected = true;
		}
	}
}
