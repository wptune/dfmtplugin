using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomMTTranslationProvider.CustomMT;

internal class CustomMTCommunication
{
	public static List<Storage.GetTemplatesResult> GetTemplates(string source, string target, string _userToken)
	{
		return JsonConvert.DeserializeObject<List<Storage.GetTemplatesResult>>(Web.SendRequest(new Web.Request
		{
			method = Web.RequestMethod.POST,
			url = Storage.urlGetTempplates,
			payloadType = Web.RequestPayloadType.JSON,
			Headers = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("token", _userToken)
			},
			payloadJson = JsonConvert.SerializeObject((object)new Storage.GetTemplateListPayload
			{
				source_language = source.ToLowerInvariant(),
				target_language = target.ToLowerInvariant()
			})
		}));
	}

	public static object GetTranslation(string template, string userToken, string[] textToTranslate)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		string text = Web.SendRequest(new Web.Request
		{
			method = Web.RequestMethod.POST,
			url = Storage.urlGetTranslation,
			payloadType = Web.RequestPayloadType.JSON,
			Headers = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("token", userToken)
			},
			payloadJson = JsonConvert.SerializeObject((object)new Storage.GetTranslationPayload
			{
				template_name = template,
				text = textToTranslate
			})
		});
		JsonSerializerSettings val = new JsonSerializerSettings
		{
			MissingMemberHandling = (MissingMemberHandling)1
		};
		try
		{
			return JsonConvert.DeserializeObject<Storage.GetTranslationResult>(text, val);
		}
		catch
		{
			return JsonConvert.DeserializeObject<Storage.Error>(text);
		}
	}
}
