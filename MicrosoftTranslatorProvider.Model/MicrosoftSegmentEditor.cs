using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using MicrosoftTranslatorProvider.Studio;
using NLog;

namespace MicrosoftTranslatorProvider.Model;

public class MicrosoftSegmentEditor
{
	private readonly string _fileName;

	private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	private EditCollection _editCollection;

	private DateTime _lastVersion;

	public MicrosoftSegmentEditor(string editCollectionFilename)
	{
		_fileName = editCollectionFilename;
		_lastVersion = File.GetLastWriteTime(_fileName);
		LoadCollection();
	}

	public void LoadCollection()
	{
		try
		{
			StreamReader textReader = new StreamReader(_fileName);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(EditCollection));
			_editCollection = xmlSerializer.Deserialize(textReader) as EditCollection;
		}
		catch (InvalidOperationException exObj)
		{
			HandleException(exObj);
		}
		catch (Exception exObj2)
		{
			HandleException(exObj2);
		}
	}

	public string EditText(string text)
	{
		string text2 = text;
		DateTime lastWriteTime = File.GetLastWriteTime(_fileName);
		if (lastWriteTime > _lastVersion)
		{
			_lastVersion = lastWriteTime;
			LoadCollection();
		}
		if (_editCollection.Items.Count == 0)
		{
			return text;
		}
		for (int i = 0; i < _editCollection.Items.Count; i++)
		{
			if (_editCollection.Items[i].Enabled)
			{
				string findText = _editCollection.Items[i].FindText;
				string replaceText = _editCollection.Items[i].ReplaceText;
				if (_editCollection.Items[i].Type == EditItemType.PlainText)
				{
					text2 = text2.Replace(findText, replaceText);
				}
				else if (_editCollection.Items[i].Type == EditItemType.RegularExpression)
				{
					Regex regex = new Regex(findText);
					text2 = regex.Replace(text2, replaceText);
				}
			}
		}
		return text2;
	}

	private IntPtr GetHandle()
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		Process[] processesByName = Process.GetProcessesByName(friendlyName.Substring(0, friendlyName.LastIndexOf('.')));
		return processesByName[0].MainWindowHandle;
	}

	private void HandleException(object exObj)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		Exception ex = exObj as Exception;
		_logger.Error(MethodBase.GetCurrentMethod().Name + " " + ex.Message + "\n " + ex.StackTrace);
		WindowWrapper windowWrapper = new WindowWrapper(GetHandle());
		string editSettingsErrorCaption = PluginResources.EditSettingsErrorCaption;
		string text = string.Format(PluginResources.EditSettingsXmlErrorMessage, Path.GetFileName(_fileName));
		string text2 = PluginResources.EditSettingsGenericErrorMessage + " " + ex.Message;
		string text3 = ((exObj is InvalidOperationException) ? text : text2);
		MessageBox.Show((IWin32Window)(object)windowWrapper, text3, editSettingsErrorCaption, (MessageBoxButtons)0, (MessageBoxIcon)48);
		throw new Exception(text3);
	}
}
