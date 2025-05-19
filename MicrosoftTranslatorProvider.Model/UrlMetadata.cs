using System.Windows.Input;
using MicrosoftTranslatorProvider.Commands;

namespace MicrosoftTranslatorProvider.Model;

public class UrlMetadata : BaseModel
{
	private ICommand _clearCommand;

	public string Key { get; set; }

	public string Value { get; set; }

	public bool IsSelected { get; set; }

	public bool IsReadOnly { get; set; }

	public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(Clear));

	private void Clear(object parameter)
	{
		if (!(parameter is string text))
		{
			return;
		}
		string text2 = text;
		string text3 = text2;
		if (!(text3 == "key"))
		{
			if (text3 == "value")
			{
				Value = string.Empty;
				OnPropertyChanged("Value");
			}
		}
		else
		{
			Key = string.Empty;
			OnPropertyChanged("Key");
		}
	}
}
