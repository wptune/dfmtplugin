using System.Collections.ObjectModel;
using MicrosoftTranslatorProvider.Model;

namespace MicrosoftTranslatorProvider.Interface;

public interface IPrivateEndpointViewModel
{
	BaseModel ViewModel { get; }

	string Endpoint { get; set; }

	ObservableCollection<UrlMetadata> Headers { get; set; }

	ObservableCollection<UrlMetadata> Parameters { get; set; }
}
