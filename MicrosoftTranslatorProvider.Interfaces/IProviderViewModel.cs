using System.Collections.Generic;
using MicrosoftTranslatorProvider.Model;
using Newtonsoft.Json;

namespace MicrosoftTranslatorProvider.Interfaces;

public interface IProviderViewModel
{
	[JsonIgnore]
	string ApiKey { get; set; }

	bool PersistMicrosoftKey { get; set; }

	BaseModel ViewModel { get; }

	RegionSubscription SelectedRegion { get; set; }

	List<RegionSubscription> Regions { get; set; }

	List<PairMapping> LanguageMappings { get; set; }
}
