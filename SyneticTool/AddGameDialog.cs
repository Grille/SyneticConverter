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
using SyneticLib;


namespace SyneticTool;

public partial class AddGameDialog : Form
{
    public string GameName;
    public string GamePath;
    public Games Games;
    public AddGameDialog(Games games)
    {
        InitializeComponent();

        Games = games;
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
        string path = textBoxPath.Text.Trim();
        if (Directory.Exists(path))
        {
            textBoxName.Text = GameFolder.GetGameVersion(path).ToString();
            GamePath = path;
            textBoxPath.ForeColor = SystemColors.WindowText;
            buttonOk.Enabled = true;
        }
        else
        {
            textBoxPath.ForeColor = Color.Red;
            buttonOk.Enabled = false;
        }
    }

    private void textBoxName_TextChanged(object sender, EventArgs e)
    {
        GameName = textBoxName.Text;
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
}
