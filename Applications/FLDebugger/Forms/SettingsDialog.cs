using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using FLDebugger.Utils;

namespace FLDebugger.Forms
{
    public partial class SettingsDialog : Form
    {
        private FLScriptEditor Editor;
        private Dictionary<string, FieldInfo> ThemeSettings;

        private FileSystemWatcher fswThemes = new FileSystemWatcher(Path.Combine(FLScriptEditor.ConfigPath, "themes"));


        public SettingsDialog(FLScriptEditor editor)
        {
            Editor = editor;
            InitializeComponent();

            InitializeThemes();

            FLScriptEditor.RegisterDefaultTheme(nudWidth);
            FLScriptEditor.RegisterDefaultTheme(gbResolution);
            FLScriptEditor.RegisterDefaultTheme(nudHeight);
            FLScriptEditor.RegisterDefaultTheme(lblHeight);
            FLScriptEditor.RegisterDefaultTheme(lblDepth);
            FLScriptEditor.RegisterDefaultTheme(nudDepth);
            FLScriptEditor.RegisterDefaultTheme(panelButtons);
            FLScriptEditor.Theme.Register(theme => btnUnpackKernels.BackColor = theme.PrimaryBackgroundColor);
            FLScriptEditor.Theme.Register(theme => btnReloadKernels.BackColor = theme.PrimaryBackgroundColor);
            FLScriptEditor.RegisterDefaultTheme(btnSetHomeDir);
            FLScriptEditor.RegisterDefaultTheme(btnAbout);
            FLScriptEditor.RegisterDefaultTheme(gbEditor);
            FLScriptEditor.RegisterDefaultTheme(panelColorPreview);
            FLScriptEditor.RegisterDefaultTheme(cbEditorColorSetting);
            FLScriptEditor.RegisterDefaultTheme(btnChangeColor);
            FLScriptEditor.RegisterDefaultTheme(lblWidth);
            FLScriptEditor.RegisterDefaultTheme(btnSaveTheme);
            FLScriptEditor.RegisterDefaultTheme(btnSaveThemeAsDefault);
            FLScriptEditor.RegisterDefaultTheme(btnLoadTheme);
            FLScriptEditor.RegisterDefaultTheme(cbLogParserStacktrace);
            FLScriptEditor.RegisterDefaultTheme(cbLogPreviewStacktrace);
        }


        private void InitializeThemes()
        {
            IEnumerable<FieldInfo> infos = FLScriptEditor.Theme.GetType().GetFields().Where(x => x.FieldType == typeof(XMLColor));
            ThemeSettings = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo fieldInfo in infos)
            {
                ThemeSettings.Add(fieldInfo.Name, fieldInfo);
            }

            cbEditorColorSetting.Items.Clear();
            cbEditorColorSetting.Items.AddRange(ThemeSettings.Keys.Cast<object>().ToArray());
            cbThemes.Items.AddRange(Directory
                .GetFiles(Path.Combine(FLScriptEditor.ConfigPath, "themes"), "*.xml", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileNameWithoutExtension).Cast<object>().ToArray());
            fswThemes.Filter = "*.xml";
            fswThemes.Created += FswThemes_Created;
            fswThemes.Renamed += FswThemes_Renamed;
            fswThemes.Deleted += FswThemes_Deleted;
        }

        private void FswThemes_Deleted(object sender, FileSystemEventArgs e)
        {
            cbThemes.Items.Remove(Path.GetFileNameWithoutExtension(e.Name));
        }

        private void FswThemes_Renamed(object sender, RenamedEventArgs e)
        {
            cbThemes.Items.Remove(Path.GetFileNameWithoutExtension(e.OldName));
            cbThemes.Items.Add(Path.GetFileNameWithoutExtension(e.Name));
        }

        private void FswThemes_Created(object sender, FileSystemEventArgs e)
        {
            cbThemes.Items.Add(Path.GetFileNameWithoutExtension(e.Name));
        }

        private void btnSetHomeDir_Click(object sender, EventArgs e)
        {
            Editor.SetWorkingDirectory();
        }

        private void btnReloadKernels_Click(object sender, EventArgs e)
        {
            Editor.ReloadKernels();
        }

        private void btnUnpackKernels_Click(object sender, EventArgs e)
        {
            Editor.UnpackResources();
            Editor.ReloadKernels();
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            Editor.SetResolution((int)nudWidth.Value, (int)nudHeight.Value, (int)nudDepth.Value);
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            Editor.SetResolution((int)nudWidth.Value, (int)nudHeight.Value, (int)nudDepth.Value);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            Editor.ShowAbout();
        }

        private void cbEditorColorSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelColorPreview.BackColor = (XMLColor)ThemeSettings[cbEditorColorSetting.SelectedItem.ToString()].GetValue(FLScriptEditor.Theme);
        }

        private void btnChangeColor_Click(object sender, EventArgs e)
        {
            cdChangeThemeColor.Color = (XMLColor)ThemeSettings[cbEditorColorSetting.SelectedItem.ToString()]
                .GetValue(FLScriptEditor.Theme);
            if (cdChangeThemeColor.ShowDialog() == DialogResult.OK)
            {
                FLScriptEditor.Theme.SetValue(ThemeSettings[cbEditorColorSetting.SelectedItem.ToString()], (XMLColor)cdChangeThemeColor.Color);
            }
        }

        private void btnSaveTheme_Click(object sender, EventArgs e)
        {
            if (sfdTheme.ShowDialog() == DialogResult.OK)
            {
                Stream s = File.Create(sfdTheme.FileName);
                XmlSerializer xs = new XmlSerializer(typeof(FLEditorTheme));
                xs.Serialize(s, FLScriptEditor.Theme);
                s.Close();
            }
        }

        private void btnSaveThemeAsDefault_Click(object sender, EventArgs e)
        {
            Stream s = File.Create(FLScriptEditor.Settings.Theme);
            XmlSerializer xs = new XmlSerializer(typeof(FLEditorTheme));
            xs.Serialize(s, FLScriptEditor.Theme);
            s.Close();
        }

        private void nudLogFontSize_ValueChanged(object sender, EventArgs e)
        {
            FLScriptEditor.Theme.SetValue(FLScriptEditor.Theme.GetType().GetField("LogFontSize"), (float)nudLogFontSize.Value);
        }

        private void nudCodeFontSize_ValueChanged(object sender, EventArgs e)
        {
            FLScriptEditor.Theme.SetValue(FLScriptEditor.Theme.GetType().GetField("CodeFontSize"), (float)nudCodeFontSize.Value);
        }

        private void btnLoadTheme_Click(object sender, EventArgs e)
        {
            if (cbThemes.SelectedIndex != -1)
            {
                XmlSerializer xs = new XmlSerializer(typeof(FLEditorTheme));
                Stream s = File.OpenRead(Path.Combine(FLScriptEditor.ConfigPath, "themes", cbThemes.SelectedItem + ".xml"));
                FLEditorTheme theme = (FLEditorTheme)xs.Deserialize(s);
                FLEditorTheme.TransferEvents(FLScriptEditor.Theme, theme);
                FLScriptEditor.Theme = theme;
                File.WriteAllText(Path.Combine(FLScriptEditor.ConfigPath, "last_theme.txt"), cbThemes.SelectedItem + ".xml");
            }
        }

        private void cbLogPreviewStacktrace_CheckedChanged(object sender, EventArgs e)
        {
            FLScriptEditor.Settings.LogProgramStacktrace = cbLogPreviewStacktrace.Checked;
        }

        private void cbLogParserStacktrace_CheckedChanged(object sender, EventArgs e)
        {
            FLScriptEditor.Settings.LogParserStacktrace = cbLogParserStacktrace.Checked;
        }

        private void nudDepth_ValueChanged(object sender, EventArgs e)
        {
            Editor.SetResolution((int)nudWidth.Value, (int)nudHeight.Value, (int)nudDepth.Value);
        }
    }
}
