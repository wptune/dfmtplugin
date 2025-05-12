using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomMTTranslationProvider.CustomMT;

internal class CustomMTCommunication
{

    public static object GetTranslation(string source, string target, string[] textToTranslate)
    {
        string text = Web.SendRequest(new Web.Request
        {
            method = Web.RequestMethod.POST,
            url = Storage.urlGetTranslation,
            payloadType = Web.RequestPayloadType.JSON,
            payloadJson = JsonConvert.SerializeObject(new
            {
                inputs = new
                {
                    source_text_input = textToTranslate,
                    source_language = source.ToLowerInvariant(),
                    target_language = target.ToLowerInvariant(),
                    subject_area = "", // Set the subject area
                    style_guide_id = "" // Set the style guide ID
                },
                response_mode = "blocking"
            })
        });
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        try
        {
            return JsonConvert.DeserializeObject<Storage.GetTranslationResult>(text, settings);
        }
        catch
        {
            return JsonConvert.DeserializeObject<Storage.Error>(text);
        }
    }
}
