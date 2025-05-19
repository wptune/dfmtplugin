using System.Windows.Input;
using MicrosoftTranslatorProvider.Commands;
using Sdl.LanguagePlatform.Core;

namespace MicrosoftTranslatorProvider.Model;

public class PairMapping : BaseModel
{
	private string _categoryId;

	private ICommand _clearCommand;

	public LanguagePair LanguagePair { get; set; }

	public string DisplayName { get; set; }

	public string CategoryID
	{
		get
		{
			return _categoryId;
		}
		set
		{
			_categoryId = value;
			OnPropertyChanged("CategoryID");
		}
	}

	public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(Clear));

	private void Clear(object parameter)
	{
		CategoryID = string.Empty;
	}
}
