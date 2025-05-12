namespace CustomMTTranslationProvider.CustomMT;

internal class ApiConnecter
{
	internal ApiConnecter()
	{}

	internal object Translate(string[] textToTranslate)
	{
		return CustomMTCommunication.GetTranslation(textToTranslate);
	}
}
