using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using MicrosoftTranslatorProvider.Extensions;
using MicrosoftTranslatorProvider.Helpers;
using MicrosoftTranslatorProvider.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MicrosoftTranslatorProvider.ApiService
{
    class PrivateEndpointApi
    {
        private readonly string _uri;
        private readonly List<UrlMetadata> _headers;
        private readonly List<UrlMetadata> _parameters;
        private string _apiKey;

        // Dictionary to map ISO language codes to full language names
        private readonly Dictionary<string, string> _languageCodeToName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "af", "Afrikaans" },
            { "sq", "Albanian" },
            { "am", "Amharic" },
            { "ar", "Arabic" },
            { "hy", "Armenian" },
            { "as", "Assamese" },
            { "az", "Azerbaijani" },
            { "eu", "Basque" },
            { "be", "Belarusian" },
            { "bn", "Bengali" },
            { "bs", "Bosnian" },
            { "bg", "Bulgarian" },
            { "my", "Burmese" },
            { "ca", "Catalan" },
            { "zh-Hans", "Chinese (Simplified)" },
            { "zh-Hant", "Chinese (Traditional)" },
            { "zh-CN", "Chinese (PRC)" },
            { "zh-TW", "Chinese (Taiwan)" },
            { "hr", "Croatian" },
            { "cs", "Czech" },
            { "da", "Danish" },
            { "nl", "Dutch" },
            { "en", "English" },
            { "et", "Estonian" },
            { "fi", "Finnish" },
            { "fr", "French" },
            { "gl", "Galician" },
            { "ka", "Georgian" },
            { "de", "German" },
            { "el", "Greek" },
            { "gu", "Gujarati" },
            { "ht", "Haitian Creole" },
            { "he", "Hebrew" },
            { "hi", "Hindi" },
            { "hu", "Hungarian" },
            { "is", "Icelandic" },
            { "id", "Indonesian" },
            { "ga", "Irish" },
            { "it", "Italian" },
            { "ja", "Japanese" },
            { "kn", "Kannada" },
            { "kk", "Kazakh" },
            { "km", "Khmer" },
            { "ko", "Korean" },
            { "ku", "Kurdish" },
            { "ky", "Kyrgyz" },
            { "lo", "Lao" },
            { "lv", "Latvian" },
            { "lt", "Lithuanian" },
            { "mk", "Macedonian" },
            { "ms", "Malay" },
            { "ml", "Malayalam" },
            { "mt", "Maltese" },
            { "mi", "Maori" },
            { "mr", "Marathi" },
            { "mn", "Mongolian" },
            { "ne", "Nepali" },
            { "nb", "Norwegian (Bokmål)" },
            { "nn", "Norwegian (Nynorsk)" },
            { "no", "Norwegian (Bokmål)" },
            { "or", "Odia" },
            { "ps", "Pashto" },
            { "fa", "Persian" },
            { "pl", "Polish" },
            { "pt", "Portuguese" },
            { "pt-BR", "Portuguese (Brazil)" },
            { "pt-PT", "Portuguese (Portugal)" },
            { "pa", "Punjabi" },
            { "ro", "Romanian" },
            { "ru", "Russian" },
            { "sr", "Serbian" },
            { "sr-Cyrl", "Serbian (Cyrillic)" },
            { "sr-Latn", "Serbian (Latin)" },
            { "sk", "Slovak" },
            { "sl", "Slovenian" },
            { "so", "Somali" },
            { "es", "Spanish" },
            { "sw", "Swahili" },
            { "sv", "Swedish" },
            { "ta", "Tamil" },
            { "te", "Telugu" },
            { "th", "Thai" },
            { "tr", "Turkish" },
            { "uk", "Ukrainian" },
            { "ur", "Urdu" },
            { "uz", "Uzbek" },
            { "vi", "Vietnamese" },
            { "cy", "Welsh" },
            { "yi", "Yiddish" },
            { "yo", "Yoruba" },
            { "zu", "Zulu" }
        };

        public PrivateEndpointApi(string endpoint, List<UrlMetadata> headers, List<UrlMetadata> parameters)
        {
            _headers = headers;
            _parameters = parameters;
            _uri = endpoint; // Use the endpoint directly as the URL

            // Extract API key from headers
            foreach (var header in headers)
            {
                if (header.Key.Equals("api_key", StringComparison.OrdinalIgnoreCase))
                {
                    _apiKey = header.Value;
                    break;
                }
            }
        }

        private string BuildUri(string endpoint)
        {
            // Not needed for the custom API, but keeping the method for compatibility
            return endpoint;
        }

        public string Translate(string sourceLanguage, string targetLanguage, string textToTranslate)
        {
            try
            {
                sourceLanguage = ConvertLanguageCode(sourceLanguage);
                targetLanguage = ConvertLanguageCode(targetLanguage);
                return TryTranslate(sourceLanguage, targetLanguage, textToTranslate);
            }
            catch (WebException exception)
            {
                ErrorHandler.HandleError(exception);
                return null;
            }
        }

        private string TryTranslate(string sourceLanguage, string targetLanguage, string textToTranslate)
        {
            const string RegexPattern = @"(\<\w+[üäåëöøßşÿÄÅÆĞ]*[^\d\W\\/\\]+\>)";
            var words = new Regex(RegexPattern).Matches(textToTranslate); //search for words like this: <example> 
            if (words.Count > 0)
            {
                textToTranslate = textToTranslate.ReplaceCharacters(words);
            }

            return RequestTranslation(sourceLanguage, targetLanguage, textToTranslate);
        }

        private string RequestTranslation(string sourceLanguage, string targetLanguage, string textToTranslate)
        {
            try
            {
                return TryRequestTranslation(sourceLanguage, targetLanguage, textToTranslate);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return null;
            }
        }

        private string TryRequestTranslation(string sourceLanguage, string targetLanguage, string textToTranslate)
        {
            // Convert ISO language codes to full language names
            string sourceLanguageName = GetLanguageName(sourceLanguage);
            string targetLanguageName = GetLanguageName(targetLanguage);

            // Create request body for Dify API
            var requestData = new
            {
                inputs = new
                {
                    source_text_input = textToTranslate,
                    source_language = sourceLanguageName,
                    target_language = targetLanguageName,
                    subject_area = "1. General",
                    style_guide_id = "style"
                },
                response_mode = "blocking",
                user = "Valentin"
            };

            var requestBody = JsonConvert.SerializeObject(requestData);
            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_uri)
            };

            // Add Authorization header with Bearer token
            httpRequest.Headers.Add("Authorization", $"Bearer {_apiKey}");

            // Add any other headers that might be needed
            foreach (var header in _headers)
            {
                if (!header.Key.Equals("api_key", StringComparison.OrdinalIgnoreCase))
                {
                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            var httpClient = new HttpClient();
            var response = httpClient.SendAsync(httpRequest).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseBody);
                    throw new Exception(responseMessage.Error.Message);
                }
                catch
                {
                    throw new Exception($"API request failed: {response.StatusCode}:\n{responseBody}");
                }
            }

            try
            {
                // Parse the Dify API response to extract the translated text
                var responseJson = JObject.Parse(responseBody);
                var translatedText = responseJson["data"]["outputs"]["text"].ToString();
                return new HtmlUtil().HtmlDecode(translatedText);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse API response: {ex.Message}. Response: {responseBody}");
            }
        }

        private string GetLanguageName(string languageCode)
        {
            if (_languageCodeToName.TryGetValue(languageCode, out string languageName))
            {
                return languageName;
            }

            // If not found in our dictionary, try to get the display name from CultureInfo
            try
            {
                var cultureInfo = new CultureInfo(languageCode);
                return cultureInfo.DisplayName;
            }
            catch
            {
                // If all else fails, return the original code
                // This might cause an API error, but at least we tried
                return languageCode;
            }
        }

        private string ConvertLanguageCode(string languageCode)
        {
            const string TraditionalChinese = "zh-TW zh-HK zh-MO zh-Hant zh-CHT";
            const string SimplifiedChinese = "zh-CN zh-SG zh-Hans-HK zh-Hans-MO zh-Hans zh-CHS";

            var cultureInfo = new CultureInfo(languageCode);
            var isSerbianCyrillic = languageCode.Contains("sr-Cyrl");
            var isSerbianLatin = languageCode.Contains("sr-Latn");
            var isTraditionalChinese = TraditionalChinese.Contains(cultureInfo.Name);
            var isSimplifiedChinese = SimplifiedChinese.Contains(cultureInfo.Name);

            if (isSerbianCyrillic)
            {
                return "sr-Cyrl";
            }
            else if (isSerbianLatin)
            {
                return "sr-Latn";
            }
            else if (isTraditionalChinese)
            {
                return "zh-Hant";
            }
            else if (isSimplifiedChinese)
            {
                return "zh-Hans";
            }

            return cultureInfo.TwoLetterISOLanguageName;
        }
    }
}
