using System;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

using SyneticLib;
using SyneticLib.IO.Synetic.Files;

namespace SyneticBasicTools;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        setupVersions(comboBoxVSrc);
        setupVersions(comboBoxVDst);
    }

    private void tabPage1_Click(object sender, EventArgs e)
    {

    }

    private void textBoxDSrc_TextChanged(object sender, EventArgs e)
    {
        var info = new DirectoryInfo(textBoxDSrc.Text);
        var gameInfo = info?.Parent?.Parent?.Parent?.Parent;

        if (gameInfo != null)
            comboBoxVSrc.SelectedItem = GameDirectory.FindDirectoryGameVersion(gameInfo.FullName);

        textBoxDSrc.ForeColor = File.Exists(textBoxDSrc.Text) ? Color.Black : Color.Red;
    }

    private void buttonBSrc_Click(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "geo files|*.idx;*.vtx;*.geo";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            textBoxDSrc.Text = dialog.FileName;
        }
    }

    private void textBoxDDst_TextChanged(object sender, EventArgs e)
    {
        textBoxDDst.ForeColor = Directory.Exists(Path.GetDirectoryName(textBoxDDst.Text)) ? Color.Black : Color.Red;
    }

    private void buttonBDst_Click(object sender, EventArgs e)
    {
        var srcpath = textBoxDSrc.Text;
        var dialog = new SaveFileDialog();
        dialog.OverwritePrompt = false;
        dialog.FileName = Path.GetFileName(Path.ChangeExtension(srcpath, "ext"));
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            textBoxDDst.Text = dialog.FileName;
        }
    }

    private void buttonConvert_Click(object sender, EventArgs e)
    {
        if (!File.Exists(textBoxDSrc.Text))
        {
            MessageBox.Show("Invalid Source Path","Parameter Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
        }
        if (!Directory.Exists(Path.GetDirectoryName(textBoxDDst.Text)))
        {
            MessageBox.Show("Invalid Ouput Path", "Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (comboBoxVSrc.SelectedItem == null)
        {
            MessageBox.Show("Invalid Src Version", "Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (comboBoxVDst.SelectedItem == null)
        {
            MessageBox.Show("Invalid Dst Version", "Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var srcpath = textBoxDSrc.Text;
            var dstpath = textBoxDDst.Text;
            var srcVer = (GameVersion)comboBoxVSrc.SelectedItem;
            var dstVer = (GameVersion)comboBoxVDst.SelectedItem;


            IVertexData srcvtx, dstvtx;
            IIndexData srcidx, dstidx;

            if (srcVer > GameVersion.WR2)
            {
                var geo = new GeoFile();
                geo.Load(Path.ChangeExtension(srcpath, "geo"));

                srcvtx = geo;
                srcidx = geo;
            }
            else
            {
                var vtx = new VtxFile();
                var idx = new IdxFile();
                vtx.Load(Path.ChangeExtension(srcpath, "vtx"));
                idx.Load(Path.ChangeExtension(srcpath, "idx"));

                srcvtx = vtx;
                srcidx = idx;
            }

            {
                var geo = new GeoFile();
                var vtx = new VtxFile();
                var idx = new IdxFile();

                if (dstVer > GameVersion.WR2)
                {
                    geo.Path = Path.ChangeExtension(dstpath, "geo");
                    if (geo.Exists && MessageBox.Show(this, "GEO file already exists. Do you want to overwrite it?", "Override Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                        return;

                    dstvtx = geo;
                    dstidx = geo;
                }
                else
                {
                    vtx.Path = Path.ChangeExtension(dstpath, "vtx");
                    idx.Path = Path.ChangeExtension(dstpath, "idx");
                    if ((vtx.Exists || idx.Exists) && MessageBox.Show(this, "VTX/IDX file already exists. Do you want to overwrite it?", "Override Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                        return;

                    dstvtx = vtx;
                    dstidx = idx;
                }

                dstvtx.IndicesOffset = srcvtx.IndicesOffset;
                dstvtx.Vertecis = srcvtx.Vertecis;
                dstidx.Polygons = srcidx.Polygons;

                if (dstVer > GameVersion.WR2)
                {
                    geo.FillHead();
                    geo.Save();
                }
                else
                {
                    vtx.Save();
                    idx.Save();
                }

            }
            MessageBox.Show(this, $"Conversion {srcVer} -> {dstVer} successful", "Conversion Successful", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        catch (Exception err)
        {
            MessageBox.Show(this, err.Message, "Convert Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void setupVersions(ComboBox target)
    {
        target.Items.Add(GameVersion.WR2);
        target.Items.Add(GameVersion.C11);

        target.SelectedItem = GameVersion.WR2;
    }
}