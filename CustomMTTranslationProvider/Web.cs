using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CustomMTTranslationProvider;

internal class Web
{
	public enum RequestMethod
	{
		GET,
		POST,
		PUT,
		DELETE
	}

	public enum RequestPayloadType
	{
		JSON,
		FORMDATA,
		FORMDATACHARSET,
		EMPTY
	}

	public class Request
	{
		public RequestMethod method;

		public string url;

		public List<KeyValuePair<string, string>> payloadForm = new List<KeyValuePair<string, string>>();

		public string payloadJson;

		public RequestPayloadType payloadType;

		public List<KeyValuePair<string, string>> Headers = new List<KeyValuePair<string, string>>();
	}

	public static string SendRequest(Request req)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			HttpClientHandler val = new HttpClientHandler();
			try
			{
				HttpClient val2 = new HttpClient((HttpMessageHandler)(object)val);
				try
				{
					int num = 0;
					while (true)
					{
						try
						{
							HttpRequestMessage val3 = new HttpRequestMessage(new HttpMethod(req.method.ToString()), req.url);
							try
							{
								if (req.payloadType != RequestPayloadType.EMPTY)
								{
									if (req.payloadType == RequestPayloadType.FORMDATA || req.payloadType == RequestPayloadType.FORMDATACHARSET)
									{
										IEnumerable<KeyValuePair<string, string>> payloadForm = req.payloadForm;
										val3.Content = (HttpContent)new FormUrlEncodedContent(payloadForm);
										if (req.payloadType == RequestPayloadType.FORMDATA)
										{
											val3.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
										}
										else
										{
											val3.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=utf-8");
										}
									}
									else
									{
										val3.Content = (HttpContent)new StringContent(req.payloadJson);
										val3.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
									}
								}
								foreach (KeyValuePair<string, string> header in req.Headers)
								{
									((HttpHeaders)val3.Headers).Add(header.Key, header.Value);
								}
								new HttpResponseMessage();
								using StreamReader streamReader = new StreamReader(val2.SendAsync(val3).GetAwaiter().GetResult()
									.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
								return streamReader.ReadToEndAsync().GetAwaiter().GetResult();
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						catch (Exception ex)
						{
							num++;
							if (num == 5)
							{
								throw ex;
							}
						}
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex2)
		{
			throw ex2;
		}
	}
}
