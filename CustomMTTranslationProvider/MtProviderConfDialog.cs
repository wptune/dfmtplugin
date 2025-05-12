using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomMTTranslationProvider.CustomMT;
using Newtonsoft.Json;
using Sdl.LanguagePlatform.Core;

namespace CustomMTTranslationProvider;

public class MtProviderConfDialog : Form
{
	private string source;

	private string target;

	private IContainer components;

	private Label label1;

	private Label label2;

	private TextBox TBuserToken;

	private Button BTNOk;

	private Label label3;

	private ComboBox CBTemplates;

	private Button BTNValidateToken;

	private Button BTNClear;

	public MtTranslationOptions _Options { get; set; }

	public void Setsettings()
	{
		try
		{
			Storage.SettingsFileData settingsFileData = new Storage.SettingsFileData
			{
				model = CBTemplates.SelectedItem.ToString(),
				source = source,
				target = target,
				token = ((Control)TBuserToken).Text
			};
			if (!Directory.Exists(Path.GetDirectoryName(Storage.SettingsFile())))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(Storage.SettingsFile()));
			}
			File.WriteAllText(Storage.SettingsFile(), JsonConvert.SerializeObject((object)settingsFileData));
		}
		catch
		{
		}
	}

	public void Getsettings()
	{
		try
		{
			if (File.Exists(Storage.SettingsFile()))
			{
				Storage.SettingsFileData settingsFileData = JsonConvert.DeserializeObject<Storage.SettingsFileData>(File.ReadAllText(Storage.SettingsFile()));
				_Options.SetToken(settingsFileData.token);
				if (source.ToLowerInvariant() == settingsFileData.source.ToLowerInvariant() && target.ToLowerInvariant() == settingsFileData.target.ToLowerInvariant())
				{
					_Options.SetModel(settingsFileData.model);
				}
			}
		}
		catch
		{
		}
	}

	public MtProviderConfDialog(LanguagePair langpair)
	{
		_Options = new MtTranslationOptions();
		InitializeComponent();
		source = ((object)langpair.SourceCulture).ToString();
		target = ((object)langpair.TargetCulture).ToString();
	}

	public MtProviderConfDialog(string token, string model, LanguagePair langpair)
	{
		_Options = new MtTranslationOptions(model, token);
		InitializeComponent();
		source = ((object)langpair.SourceCulture).ToString();
		target = ((object)langpair.TargetCulture).ToString();
	}

	private void UpdateDialogObjects()
	{
		if (!string.IsNullOrEmpty(_Options.GetToken()))
		{
			((Control)TBuserToken).Text = _Options.GetToken();
			ButtonValidateTokenClicked();
		}
		if (!string.IsNullOrEmpty(_Options.GetModel()) && CBTemplates.Items.Contains((object)_Options.GetModel()))
		{
			CBTemplates.SelectedItem = _Options.GetModel();
		}
	}

	private void TBuserToken_TextChanged(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(((Control)TBuserToken).Text))
		{
			((Control)BTNValidateToken).Enabled = false;
		}
		else
		{
			((Control)BTNValidateToken).Enabled = true;
		}
	}

	private void ButtonValidateTokenClicked()
	{
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		string text = ((Control)TBuserToken).Text;
		try
		{
			List<Storage.GetTemplatesResult> templates = CustomMTCommunication.GetTemplates(source, target, text);
			if (templates.Count() > 0)
			{
				((Control)CBTemplates).Enabled = true;
				CBTemplates.Items.Clear();
				foreach (Storage.GetTemplatesResult item in templates)
				{
					CBTemplates.Items.Add((object)item.template_name);
				}
				((ListControl)CBTemplates).SelectedIndex = 0;
				((Control)TBuserToken).Enabled = false;
				((Control)BTNValidateToken).Enabled = false;
			}
			else
			{
				CBTemplates.Items.Clear();
				((Control)CBTemplates).Enabled = false;
				((Control)TBuserToken).Enabled = true;
				((Control)BTNValidateToken).Enabled = true;
				MessageBox.Show("The token is invalid or your account does not have any templates for the specific language pair. Please check and try again.");
			}
		}
		catch
		{
			CBTemplates.Items.Clear();
			((Control)CBTemplates).Enabled = false;
			((Control)TBuserToken).Enabled = true;
			((Control)BTNValidateToken).Enabled = true;
			MessageBox.Show("The token is invalid or your account does not have any templates for the specific language pair. Please check and try again.");
		}
	}

	private void BTNValidateToken_Click(object sender, EventArgs e)
	{
		ButtonValidateTokenClicked();
	}

	private void CBTemplates_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (CBTemplates.Items.Count > 0 && ((ListControl)CBTemplates).SelectedIndex >= 0)
		{
			((Control)BTNOk).Enabled = true;
		}
		else
		{
			((Control)BTNOk).Enabled = false;
		}
	}

	private void BTNOk_Click(object sender, EventArgs e)
	{
		if (!ValidateForm())
		{
			((Form)this).DialogResult = (DialogResult)2;
			return;
		}
		_Options.SetToken(((Control)TBuserToken).Text);
		_Options.SetModel(CBTemplates.SelectedItem.ToString());
		Setsettings();
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private bool ValidateForm()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		if (((Control)TBuserToken).Text == string.Empty)
		{
			MessageBox.Show("No user token has been set");
			result = false;
		}
		if (CBTemplates.Items.Count <= 0 || ((ListControl)CBTemplates).SelectedIndex < 0)
		{
			MessageBox.Show("No template has been set");
			result = false;
		}
		return result;
	}

	private void MtProviderConfDialog_Shown(object sender, EventArgs e)
	{
		Getsettings();
		UpdateDialogObjects();
	}

	private void BTNClear_Click(object sender, EventArgs e)
	{
		((Control)TBuserToken).Enabled = true;
		((Control)BTNValidateToken).Enabled = true;
		((Control)CBTemplates).Enabled = false;
		((Control)BTNOk).Enabled = true;
		((Control)TBuserToken).Text = "";
		CBTemplates.Items.Clear();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MtProviderConfDialog));
		label1 = new Label();
		label2 = new Label();
		TBuserToken = new TextBox();
		BTNOk = new Button();
		label3 = new Label();
		CBTemplates = new ComboBox();
		BTNValidateToken = new Button();
		BTNClear = new Button();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).Location = new Point(15, 9);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(153, 20);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "Custom.MT Settings";
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).Location = new Point(15, 51);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(125, 15);
		((Control)label2).TabIndex = 1;
		((Control)label2).Text = "Enter your user token:";
		((Control)TBuserToken).Location = new Point(15, 73);
		((Control)TBuserToken).Name = "TBuserToken";
		((Control)TBuserToken).Size = new Size(326, 20);
		((Control)TBuserToken).TabIndex = 2;
		((Control)TBuserToken).TextChanged += TBuserToken_TextChanged;
		((Control)BTNOk).Enabled = false;
		((Control)BTNOk).Location = new Point(322, 189);
		((Control)BTNOk).Name = "BTNOk";
		((Control)BTNOk).Size = new Size(75, 23);
		((Control)BTNOk).TabIndex = 3;
		((Control)BTNOk).Text = "OK";
		((ButtonBase)BTNOk).UseVisualStyleBackColor = true;
		((Control)BTNOk).Click += BTNOk_Click;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).Location = new Point(15, 115);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(162, 15);
		((Control)label3).TabIndex = 4;
		((Control)label3).Text = "Select the model to be used:";
		CBTemplates.DropDownStyle = (ComboBoxStyle)2;
		((Control)CBTemplates).Enabled = false;
		((ListControl)CBTemplates).FormattingEnabled = true;
		((Control)CBTemplates).Location = new Point(15, 137);
		((Control)CBTemplates).Name = "CBTemplates";
		((Control)CBTemplates).Size = new Size(382, 21);
		((Control)CBTemplates).TabIndex = 5;
		CBTemplates.SelectedIndexChanged += CBTemplates_SelectedIndexChanged;
		((Control)BTNValidateToken).Enabled = false;
		((Control)BTNValidateToken).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BTNValidateToken).Location = new Point(347, 73);
		((Control)BTNValidateToken).Name = "BTNValidateToken";
		((Control)BTNValidateToken).Size = new Size(50, 20);
		((Control)BTNValidateToken).TabIndex = 6;
		((Control)BTNValidateToken).Text = "Verify";
		((ButtonBase)BTNValidateToken).UseVisualStyleBackColor = true;
		((Control)BTNValidateToken).Click += BTNValidateToken_Click;
		((Control)BTNClear).Location = new Point(241, 189);
		((Control)BTNClear).Name = "BTNClear";
		((Control)BTNClear).Size = new Size(75, 23);
		((Control)BTNClear).TabIndex = 7;
		((Control)BTNClear).Text = "Clear";
		((ButtonBase)BTNClear).UseVisualStyleBackColor = true;
		((Control)BTNClear).Click += BTNClear_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(401, 224);
		((Control)this).Controls.Add((Control)(object)BTNClear);
		((Control)this).Controls.Add((Control)(object)BTNValidateToken);
		((Control)this).Controls.Add((Control)(object)CBTemplates);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)BTNOk);
		((Control)this).Controls.Add((Control)(object)TBuserToken);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "MtProviderConfDialog";
		((Control)this).Text = "Custom.MT Settings";
		((Form)this).TopMost = true;
		((Form)this).Shown += MtProviderConfDialog_Shown;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
