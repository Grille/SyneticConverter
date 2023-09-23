using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI
{
    public partial class PipelinesControl : UserControl
    {
        public AsyncPipelineExecuter Executer { get; }

        public PipelineList Pipelines { get; }

        public Pipeline SelectedItem
        {
            get => ListBox.SelectedItem;
            set => ListBox.SelectedItem = value;
        }

        public PipelineTasksControl TasksControl { get; set; }

        public TextBoxDialog TextBoxDialog { get; }

        public PipelinesControl()
        {
            InitializeComponent();
            Executer = new AsyncPipelineExecuter();
            Pipelines = new PipelineList();

            TextBoxDialog = new TextBoxDialog();

            Executer.Runtime.PositionChanged += Executer_PositionChanged;

            Executer.ExecutionDone += Executer_ExecutionDone;
        }

        bool invalidated = false;

        private void Executer_PositionChanged(object sender, EventArgs e)
        {
            invalidated = true;
        }

        private void Executer_ExecutionDone(object sender, EventArgs e)
        {
            invalidated = true;
            // Called from executer task
            Invoke(() =>
            {
                UpdateEnabledRuntimeActions();
            });
        }

        public void UpdateEnabledActions()
        {
            bool single = SelectedItem != null;

            copyToolStripMenuItem.Enabled = ButtonCopy.Enabled = single;
            editToolStripMenuItem.Enabled = ButtonEdit.Enabled = single;
            deleteToolStripMenuItem.Enabled = ButtonRemove.Enabled = single;

            upToolStripMenuItem.Enabled = ButtonUp.Enabled = single;
            downToolStripMenuItem.Enabled = ButtonDown.Enabled = single;

            UpdateEnabledRuntimeActions();

            if (TasksControl == null)
                return;

            TasksControl.Enabled = single;
            TasksControl.Pipeline = SelectedItem;
            TasksControl.InvalidateItems(false);
        }

        public void UpdateEnabledRuntimeActions()
        {
            bool single = SelectedItem != null;
            bool running = Executer.Running;

            runToolStripMenuItem.Enabled = ButtonRun.Enabled = single && !running;
            stopToolStripMenuItem.Enabled = ButtonStop.Enabled = running;
        }

        public void InvalidateItems(bool save = true)
        {
            ListBox.UpdateItems(Pipelines);
            UpdateEnabledActions();

            //if (save)
            //    Pipelines.Save();
        }

        private void ListBoxIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabledActions();
        }

        private void RunClick(object sender, EventArgs e)
        {
            Executer.Execute(SelectedItem);
            UpdateEnabledActions();
            TasksControl.SetEnabledActions();
        }

        private void StopClick(object sender, EventArgs e)
        {
            Executer.Cancel();
        }

        private void NewClick(object sender, EventArgs e)
        {
            var result = TextBoxDialog.ShowDialog(ParentForm, "New Pipeline", "Pipeline");
            if (result == DialogResult.OK)
            {
                string newname = TextBoxDialog.TextResult.Trim();
                string uname = Pipelines.GetUniqueName(newname);
                var pipeline = Pipelines.CreateUnbound(uname);
                Pipelines.InsertAfter(SelectedItem, pipeline);
                InvalidateItems();
                SelectedItem = pipeline;
            }
        }

        private void CopyClick(object sender, EventArgs e)
        {
            var clone = SelectedItem.Clone();
            Pipelines.InsertAfter(SelectedItem, clone);
            InvalidateItems();
        }

        private void EditClick(object sender, EventArgs e)
        {
            string name = SelectedItem.Name;
            var result = TextBoxDialog.ShowDialog(ParentForm, "New Pipeline", name);
            if (result == DialogResult.OK)
            {
                string newname = TextBoxDialog.TextResult.Trim();
                if (newname == name)
                    return;

                string uname = Pipelines.GetUniqueName(newname);
                Pipelines.Rename(name, uname);
                ListBox.Items.Add(0);
                InvalidateItems();
            }
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            Pipelines.Remove(ListBox.SelectedItem);
            InvalidateItems();
        }

        private void UpClick(object sender, EventArgs e)
        {
            var selected = SelectedItem;
            Pipelines.UpItem(selected);
            InvalidateItems();
        }

        private void DownClick(object sender, EventArgs e)
        {
            var selected = SelectedItem;
            Pipelines.DownItem(selected);
            InvalidateItems();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (invalidated)
            {
                invalidated = false;
                ListBox.Invalidate();
                TasksControl.ListBox.Invalidate();
            }
        }

        private void ListBoxDoubleClick(object sender, EventArgs e)
        {
            if (SelectedItem == null)
                return;
            EditClick(sender, e);
        }

        private void ControlLoad(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }
    }
}
