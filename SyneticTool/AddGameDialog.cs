using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using SyneticLib.Locations;

using SyneticLib.LowLevel;

namespace SyneticTool;

public partial class AddGameDialog : Form
{
    public string NewGamePath;
    public GameVersion NewGameVersion;
    public GameDirectoryList Games;

    public GameDirectory SelectedGame;

    public AddGameDialog(GameDirectoryList games)
    {
        InitializeComponent();

        Games = games;

        var citems = comboBoxVersion.Items;
        citems.Add(GameVersion.None);
        citems.Add(GameVersion.NICE1);
        citems.Add(GameVersion.NICE2);
        citems.Add(GameVersion.MBTR);
        citems.Add(GameVersion.WR1);
        citems.Add(GameVersion.WR2);
        citems.Add(GameVersion.C11);
        citems.Add(GameVersion.CT1);
        citems.Add(GameVersion.CT2);
        citems.Add(GameVersion.CT3);
        citems.Add(GameVersion.CT4);
        citems.Add(GameVersion.CT5);

        comboBoxVersion.SelectedItem = GameVersion.None;

    }

    public AddGameDialog(GameDirectoryList games, GameDirectory selected) : this(games)
    {
        Games = games;
        textBoxPath.Text = selected.Path;
        comboBoxVersion.SelectedItem = selected.Version;
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void buttonPath_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        dialog.ShowDialog(this);

        if (dialog.SelectedPath != "")
            textBoxPath.Text = dialog.SelectedPath;

    }

    private void textBoxPath_TextChanged(object sender, EventArgs e)
    {
        NewGamePath = textBoxPath.Text.Trim();
        SelectedGame = Games.GetOrCreateEntry(NewGamePath);
        comboBoxVersion.SelectedItem = GameDirectory.GetDirectoryGameVersion(NewGamePath);

        if (Directory.Exists(NewGamePath))
        {
            textBoxPath.ForeColor = SystemColors.WindowText;
        }
        else
        {
            textBoxPath.ForeColor = Color.Red;
        }
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void comboBoxVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        NewGameVersion = (GameVersion)comboBoxVersion.SelectedItem;
    }

    public void ApplyToGame()
    {
        SelectedGame.Path = NewGamePath;
        SelectedGame.Version = NewGameVersion;
        SelectedGame.Seek();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        comboBoxVersion.SelectedItem = GameDirectory.GetDirectoryGameVersion(NewGamePath);
    }
}
