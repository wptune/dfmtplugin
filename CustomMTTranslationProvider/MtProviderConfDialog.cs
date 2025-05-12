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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CustomMTTranslationProvider;

public class MtProviderConfDialog : Form
{
	private string source;

	private string target;

    private IContainer components = new Container();

    private Label label1;

	private Label label2;

	private Button BTNOk;

	private Label label3;
    private TextBox textBoxBaseUrl;
    private Label label4;
    private Label label5;
    private TextBox textBoxPath;
    private Label label6;
    private TextBox textBoxStyleguide;
    private ComboBox comboBoxSubject;
    private TextBox textBoxApiKey;
    private Button BTNValidateToken;
    private Button BTNClear;

	public MtTranslationOptions _Options { get; set; }


    private void InitializeComboBoxSubject()
    {
        var subjects = new List<string> { "1. General", "2. Household", "2.1 Household (accessories, furniture)", "2.2 Household appliances manuals", "3. Medical Technology", "3.1 Diagnostic equipment", "3.2 Healthcare software", "3.3 Medical Lab Equipment", "3.4 Medical Cleaning and Sterilization", "3.5 Personal protective equipment", "3.6 Home Medical Equipment", "3.7 Hospital furniture and accessories", "3.8 Medical Rescue equipment", "3.9 Medical optical equipment", "3.10 Dental implants", "3.11 Orthopedic implants", "3.12 Vascular implants", "3.13 Dental Technology", "4. Technology & Engineering", "4.1 Mechanical & Plant Engineering", "4.2 Waste technology", "4.3 Building Technology", "4.4 Printing Technology", "4.5 Electrical and electronics engineering", "4.6 Energy Technology", "4.7 Vehicle Technology", "4.8 Fabrication Technology", "4.9 Conveying Technology", "4.10 Hand and Power Tools", "4.11 Climate Technology", "4.12 Plastics Technology", "4.13 Food Technology", "4.14 Logistics", "4.15 Measurement Technology", "4.16 Welding Technology", "4.17 Textile Technology", "4.18 Packaging Technology", "5. Life Science", "5.1 Clinical studies", "5.2 Chemistry", "5.3 Pharmacology", "5.4 Medical products", "5.5 Medicine & Healthcare", "5.6 Nutrition Science", "5.7 Veterinary Medicine", "5.8 Leaflets", "6. Information Technology (IT)", "6.1 Software Development", "6.2 Software Localization", "6.3 Software manuals", "7. Marketing", "7.1 Tourism", "7.2 Sports & Entertainment", "7.3 Fashion, Lifestyle & Design", "7.4 Advertising", "7.5 Publications", "7.6 Press Releases", "8. Legal", "8.1 Contracts & Agreements", "8.2 Legal regulations, standards and laws", "8.3 Criminal & Civil Law", "8.4 Notarial & Official Certificates", "9. Finances / Accounting", "10. Social", "10.1 Politics", "10.2 Social Sciences", "10.3 Religion", "10.4 Education", "10.5 Questionnaires and surveys", "10.6 Insurance", "11. Food", "12. Animals" };
        comboBoxSubject.Items.Clear();
        comboBoxSubject.Items.AddRange(subjects.ToArray());

    }

    public void Setsettings()
	{
		try
		{
			Storage.SettingsFileData settingsFileData = new Storage.SettingsFileData
			{
				//model = CBTemplates.SelectedItem.ToString(),
				baseurl = textBoxBaseUrl.Text,
				path = textBoxPath.Text,
                apikey = textBoxApiKey.Text,
                subject = comboBoxSubject.SelectedItem?.ToString(),
                styleguide = textBoxStyleguide.Text,
                source = source,
				target = target,
				 
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
                _Options.SetBaseUrl(settingsFileData.baseurl);
                _Options.SetPath(settingsFileData.path);
                _Options.SetApiKey(settingsFileData.apikey);
                _Options.SetSubject(settingsFileData.subject);
                _Options.SetStyleguide(settingsFileData.styleguide);
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
        InitializeComboBoxSubject();
        source = ((object)langpair.SourceCulture).ToString();
		target = ((object)langpair.TargetCulture).ToString();
	}

	public MtProviderConfDialog(string token, string model, LanguagePair langpair)
	{
		_Options = new MtTranslationOptions(model, token);
		InitializeComponent();
        InitializeComboBoxSubject();
        source = ((object)langpair.SourceCulture).ToString();
		target = ((object)langpair.TargetCulture).ToString();
	}

	private void UpdateDialogObjects()
	{
		if (!string.IsNullOrEmpty(_Options.GetToken()))
		{
			((Control)textBoxApiKey).Text = _Options.GetToken();
			ButtonValidateTokenClicked();
		}
		if (!string.IsNullOrEmpty(_Options.GetModel()) && comboBoxSubject.Items.Contains((object)_Options.GetModel()))
		{
			comboBoxSubject.SelectedItem = _Options.GetModel();
		}
	}

	private void TBuserToken_TextChanged(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(((Control)textBoxApiKey).Text))
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
		string text = ((Control)textBoxApiKey).Text;
		try
		{
			List<Storage.GetTemplatesResult> templates = CustomMTCommunication.GetTemplates(source, target, text);
			if (templates.Count() > 0)
			{
				((Control)comboBoxSubject).Enabled = true;
				comboBoxSubject.Items.Clear();
				foreach (Storage.GetTemplatesResult item in templates)
				{
					comboBoxSubject.Items.Add((object)item.template_name);
				}
				((ListControl)comboBoxSubject).SelectedIndex = 0;
				((Control)textBoxApiKey).Enabled = false;
				((Control)BTNValidateToken).Enabled = false;
			}
			else
			{
				comboBoxSubject.Items.Clear();
				((Control)comboBoxSubject).Enabled = false;
				((Control)textBoxApiKey).Enabled = true;
				((Control)BTNValidateToken).Enabled = true;
				MessageBox.Show("The token is invalid or your account does not have any templates for the specific language pair. Please check and try again.");
			}
		}
		catch
		{
			comboBoxSubject.Items.Clear();
			((Control)comboBoxSubject).Enabled = false;
			((Control)textBoxApiKey).Enabled = true;
			((Control)BTNValidateToken).Enabled = true;
			MessageBox.Show("The token is invalid or your account does not have any templates for the specific language pair. Please check and try again.");
		}
	}

	private void BTNValidateToken_Click(object sender, EventArgs e)
	{
		ButtonValidateTokenClicked();
	}

	private void comboBoxSubject_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboBoxSubject.Items.Count > 0 && ((ListControl)comboBoxSubject).SelectedIndex >= 0)
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
		_Options.SetToken(((Control)textBoxApiKey).Text);
		_Options.SetModel(comboBoxSubject.SelectedItem.ToString());
		Setsettings();
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private bool ValidateForm()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		if (((Control)textBoxApiKey).Text == string.Empty)
		{
			MessageBox.Show("No user token has been set");
			result = false;
		}
		if (comboBoxSubject.Items.Count <= 0 || ((ListControl)comboBoxSubject).SelectedIndex < 0)
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
		((Control)textBoxApiKey).Enabled = true;
		((Control)BTNValidateToken).Enabled = true;
		((Control)comboBoxSubject).Enabled = false;
		((Control)BTNOk).Enabled = true;
		((Control)textBoxApiKey).Text = "";
		comboBoxSubject.Items.Clear();
	}

	private void InitializeComponent()
	{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BTNOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.BTNClear = new System.Windows.Forms.Button();
            this.textBoxBaseUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxStyleguide = new System.Windows.Forms.TextBox();
            this.comboBoxSubject = new System.Windows.Forms.ComboBox();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.BTNValidateToken = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Custom.LTW Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Api key:";
            // 
            // BTNOk
            // 
            this.BTNOk.Enabled = false;
            this.BTNOk.Location = new System.Drawing.Point(322, 378);
            this.BTNOk.Name = "BTNOk";
            this.BTNOk.Size = new System.Drawing.Size(75, 24);
            this.BTNOk.TabIndex = 3;
            this.BTNOk.Text = "OK";
            this.BTNOk.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Subject:";
            // 
            // BTNClear
            // 
            this.BTNClear.Location = new System.Drawing.Point(241, 378);
            this.BTNClear.Name = "BTNClear";
            this.BTNClear.Size = new System.Drawing.Size(75, 24);
            this.BTNClear.TabIndex = 7;
            this.BTNClear.Text = "Clear";
            this.BTNClear.UseVisualStyleBackColor = true;
            // 
            // textBoxBaseUrl
            // 
            this.textBoxBaseUrl.Location = new System.Drawing.Point(15, 74);
            this.textBoxBaseUrl.Name = "textBoxBaseUrl";
            this.textBoxBaseUrl.Size = new System.Drawing.Size(382, 20);
            this.textBoxBaseUrl.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Base Url:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Path:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(16, 135);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(382, 20);
            this.textBoxPath.TabIndex = 10;
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 303);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Style guide:";
            // 
            // textBoxStyleguide
            // 
            this.textBoxStyleguide.Location = new System.Drawing.Point(16, 328);
            this.textBoxStyleguide.Name = "textBoxStyleguide";
            this.textBoxStyleguide.Size = new System.Drawing.Size(382, 20);
            this.textBoxStyleguide.TabIndex = 12;
            // 
            // comboBoxSubject
            // 
            this.comboBoxSubject.FormattingEnabled = true;
            this.comboBoxSubject.Location = new System.Drawing.Point(16, 265);
            this.comboBoxSubject.Name = "comboBoxSubject";
            this.comboBoxSubject.Size = new System.Drawing.Size(381, 21);
            this.comboBoxSubject.TabIndex = 14;
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(16, 200);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(382, 20);
            this.textBoxApiKey.TabIndex = 15;
            // 
            // BTNValidateToken
            // 
            this.BTNValidateToken.Location = new System.Drawing.Point(160, 378);
            this.BTNValidateToken.Name = "BTNValidateToken";
            this.BTNValidateToken.Size = new System.Drawing.Size(75, 24);
            this.BTNValidateToken.TabIndex = 16;
            this.BTNValidateToken.Text = "Check";
            this.BTNValidateToken.UseVisualStyleBackColor = true;
            // 
            // MtProviderConfDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 420);
            this.Controls.Add(this.BTNValidateToken);
            this.Controls.Add(this.textBoxApiKey);
            this.Controls.Add(this.comboBoxSubject);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxStyleguide);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxBaseUrl);
            this.Controls.Add(this.BTNClear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BTNOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MtProviderConfDialog";
            this.Text = "Custom.MT Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

	}

    private void label5_Click(object sender, EventArgs e)
    {

    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {

    }
}
