namespace CustomMTTranslationProvider.CustomMT;

internal class ApiConnecter
{
	private string _userToken = "";

	public MtTranslationOptions _options;

	private string model = "";

	internal ApiConnecter(MtTranslationOptions options)
	{
		_options = options;
		_userToken = options.GetToken();
		model = options.GetModel();
	}

	internal object Translate(string[] textToTranslate)
	{
		return CustomMTCommunication.GetTranslation(model, _userToken, textToTranslate);
	}
}
