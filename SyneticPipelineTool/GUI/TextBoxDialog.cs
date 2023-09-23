using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SyneticPipelineTool;

public partial class TextBoxDialog : Form
{
    public TextBoxDialog()
    {
        InitializeComponent();
    }

    public void Init(string title, string defaultText = "")
    {
        Text = title;
        textBox1.Text = defaultText;
    }

    public DialogResult ShowDialog(Form owner, string title, string defaultText = "")
    {
        Init(title, defaultText);
        return ShowDialog(owner);
    }

    public string TextResult
    {
        get => textBox1.Text;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        DialogResult= DialogResult.Cancel;
        Close();
    }
}
