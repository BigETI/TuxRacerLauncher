using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;

namespace TuxRacerLauncher
{
    public partial class FormLauncher : Form
    {
        private TuxRacerSettings settings = new TuxRacerSettings();
        private bool ask_settings = false;
        private SoundPlayer sound_player = new SoundPlayer();

        public FormLauncher()
        {
            InitializeComponent();
            settings.Load("./config/options.txt");
            updateSettings();
            sound_player.Stream = Properties.Resources.options_main;
        }

        private void checkBoxAssign(CheckBox cb)
        {
            TuxRacerSettingsItem item = settings.get((string)(cb.Tag));
            if (item != null)
            {
                item.SetToolTip(cb);
                cb.Checked = item.GetValue<bool>();
            }
        }

        private void comboBoxAssign(ComboBox cb)
        {
            TuxRacerSettingsItem item = settings.get((string)(cb.Tag));
            int index;
            if (item != null)
            {
                item.SetToolTip(cb);
                index = item.GetValue<int>();
                if ((index >= 0) && (cb.Items.Count > index))
                    cb.SelectedIndex = index;
            }
        }

        private void trackBarAssign(TrackBar tb)
        {
            TuxRacerSettingsItem item = settings.get((string)(tb.Tag));
            int value;
            if (item != null)
            {
                item.SetToolTip(tb);
                value = item.GetValue<int>();
                if ((value >= tb.Minimum) && (tb.Maximum >= value))
                    tb.Value = value;
            }
        }

        private void numericUpDownAssign(NumericUpDown nud)
        {
            TuxRacerSettingsItem item = settings.get((string)(nud.Tag));
            int value;
            if (item != null)
            {
                item.SetToolTip(nud);
                value = item.GetValue<int>();
                if ((value >= nud.Minimum) && (nud.Maximum >= value))
                    nud.Value = value;
            }
        }

        private void textBoxDownAssign(TextBox tb)
        {
            TuxRacerSettingsItem item = settings.get((string)(tb.Tag));
            if (item != null)
            {
                item.SetToolTip(tb);
                tb.Text = item.GetValue<string>();
            }
        }

        private void updateSettings()
        {
            TuxRacerSettingsItem width_i = settings.get("set x_resolution"), height_i = settings.get("set y_resolution"), bpp_i = settings.get("set bpp_mode");
            int width = 0, height = 0, bpp = 0;
            if ((width_i != null) && (height_i != null) && (bpp_i != null))
            {
                width = width_i.GetValue<int>();
                height = height_i.GetValue<int>();
                bpp = bpp_i.GetValue<int>();
                ToolTip tt = new ToolTip();
                tt.AutoPopDelay = 5000;
                tt.InitialDelay = 1000;
                tt.ReshowDelay = 500;
                tt.ShowAlways = true;
                tt.SetToolTip(comboBoxResolutions, width_i.Comments + height_i.Comments);
            }
            checkBoxAssign(checkBoxFullscreen);
            checkBoxAssign(checkBoxCaptureMouse);
            checkBoxAssign(checkBoxForceWindowPosition);
            comboBoxAssign(comboBoxViewMode);
            comboBoxResolutions.Items.Clear();
            checkBoxAssign(checkBoxNoAudio);
            checkBoxAssign(checkBoxSoundEnabled);
            checkBoxAssign(checkBoxMusicEnabled);
            trackBarAssign(trackBarSoundVolume);
            trackBarAssign(trackBarMusicVolume);
            comboBoxAssign(comboBoxAudioFrequency);
            comboBoxAssign(comboBoxAudioFormat);
            checkBoxAssign(checkBoxStereo);
            numericUpDownAssign(numericUpDownBufferSize);
            checkBoxAssign(checkBoxDisplayFPS);
            numericUpDownAssign(numericUpDownDetailLevel);
            numericUpDownAssign(numericUpDownForwardClipDistance);
            numericUpDownAssign(numericUpDownBackwardClipDistance);
            numericUpDownAssign(numericUpDownTreeDetailDistance);
            checkBoxAssign(checkBoxTerrainBlending);
            checkBoxAssign(checkBoxPerfectTerrainBlending);
            checkBoxAssign(checkBoxTerrainEnvironmentMap);
            checkBoxAssign(checkBoxDisableFog);
            checkBoxAssign(checkBoxDrawTuxShadow);
            numericUpDownAssign(numericUpDownTuxSphereDivision);
            numericUpDownAssign(numericUpDownTuxShadowDivision);
            checkBoxAssign(checkBoxDrawParticles);
            checkBoxAssign(checkBoxTrackMarks);
            checkBoxAssign(checkBoxUISnow);
            checkBoxAssign(checkBoxNiceFog);
            checkBoxAssign(checkBoxUseCVA);
            checkBoxAssign(checkBoxCVAHack);
            checkBoxAssign(checkBoxUseSphereDisplayList);
            checkBoxAssign(checkBoxDoIntroAnimation);
            comboBoxAssign(comboBoxMipmapType);
            comboBoxAssign(comboBoxODESolver);
            numericUpDownAssign(numericUpDownFOV);
            textBoxDownAssign(textBoxDebug);
            numericUpDownAssign(numericUpDownWarningLevel);
            checkBoxAssign(checkBoxWriteDiagnosticLog);
            foreach (Resolution i in DisplaySettings.GetResolutions())
            {
                comboBoxResolutions.Items.Add(i);
                if ((width == i.Width) && (height == i.Height) && (((bpp == 0) ? Screen.PrimaryScreen.BitsPerPixel : ((bpp == 2) ? 32 : 16)) == i.BPP))
                {
                    if (comboBoxResolutions.SelectedItem != null)
                    {
                        if (((Resolution)(comboBoxResolutions.SelectedItem)).Frequency < i.Frequency)
                            comboBoxResolutions.SelectedItem = i;
                    }
                    else
                        comboBoxResolutions.SelectedItem = i;
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabPageSettings;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("tuxracer.exe");
                Close();
            }
            catch
            {
                MessageBox.Show("You can't run a game, that isn't in the directory of this launcher.", "Missing Tux Racer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxGeneric_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            settings.add((string)(cb.Tag), cb.Checked, false);
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e)
        {
            settings.Save("./config/options.txt");
        }

        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            settings.Save("./config/options.txt");
            ask_settings = false;
            tabControlMain.SelectedTab = tabPageGame;
        }

        private void buttonSettingsLoadDefaults_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to set back to default settings values?", "Default settings values", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            if (result == DialogResult.Yes)
            {
                settings.LoadDefaultValues();
                updateSettings();
            }
        }

        private void comboBoxResolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            Resolution resolution = (Resolution)(comboBoxResolutions.SelectedItem);
            settings.add("set x_resolution", resolution.Width, false);
            settings.add("set y_resolution", resolution.Height, false);
            settings.add("set bpp_mode", (resolution.BPP == 32) ? 2 : 1, false);
        }

        private void comboBoxGeneric_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            settings.add((string)(cb.Tag), (cb.SelectedIndex < 0) ? 0 : cb.SelectedIndex, false);
        }

        private void checkBoxNoAudio_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = !(checkBoxNoAudio.Checked);
            checkBoxSoundEnabled.Enabled = enabled;
            checkBoxMusicEnabled.Enabled = enabled;
            labelSoundVolume.Enabled = enabled;
            trackBarSoundVolume.Enabled = enabled;
            labelMusicVolume.Enabled = enabled;
            trackBarMusicVolume.Enabled = enabled;
            labelAudioFrequency.Enabled = enabled;
            comboBoxAudioFrequency.Enabled = enabled;
            labelAudioFormat.Enabled = enabled;
            comboBoxAudioFormat.Enabled = enabled;
            checkBoxStereo.Enabled = enabled;
            labelBufferSize.Enabled = enabled;
            numericUpDownBufferSize.Enabled = enabled;
            checkBoxGeneric_CheckedChanged(sender, e);
        }

        private void trackBarSoundVolume_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            settings.add((string)(tb.Tag), tb.Value, false);
        }

        private void numericUpDownGeneric_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            settings.add((string)(nud.Tag), (int)(nud.Value), false);
        }

        private void tabControlMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            DialogResult result;
            if (e.TabPage == tabPageGame)
            {
                if (ask_settings)
                {
                    result = MessageBox.Show("Do you want to save the changes, before you leave the settings page?", "Save settings", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    if (result == DialogResult.Yes)
                        settings.Save("./config/options.txt");
                    else if (result == DialogResult.Cancel)
                        e.Cancel = true;
                }
            }
            else
            {
                updateSettings();
                ask_settings = true;
            }
        }

        private void checkBoxTerrainBlending_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBoxTerrainBlending.Checked;
            checkBoxPerfectTerrainBlending.Enabled = enabled;
            checkBoxGeneric_CheckedChanged(sender, e);
        }

        private void checkBoxDrawTuxShadow_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBoxDrawTuxShadow.Checked;
            labelTuxShadowDivision.Enabled = enabled;
            numericUpDownTuxShadowDivision.Enabled = enabled;
            checkBoxGeneric_CheckedChanged(sender, e);
        }

        private void textBoxGeneric_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            settings.add((string)(tb.Tag), tb.Text, false);
        }

        private void FormLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sound_player != null)
            {
                sound_player.Dispose();
                sound_player = null;
            }
        }

        private void FormLauncher_Activated(object sender, EventArgs e)
        {
            if (sound_player != null)
                sound_player.PlayLooping();
        }

        private void FormLauncher_Deactivate(object sender, EventArgs e)
        {
            if (sound_player != null)
                sound_player.Stop();
        }
    }
}
