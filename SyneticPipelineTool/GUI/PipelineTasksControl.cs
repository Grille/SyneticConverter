using SyneticPipelineTool.Tasks;
using System;
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
    public partial class PipelineTasksControl : UserControl
    {
        public Pipeline Pipeline { get; set; }

        public PipelineTask SelectedItem
        {
            get => ListBox.SelectedItem;
            set => ListBox.SelectedItem = value;
        }

        public IReadOnlyList<PipelineTask> SelectedItems
        {
            get => ListBox.SelectedItems;
            set => ListBox.SelectedItems = value;
        }

        EditTaksForm EditTaksForm { get; }


        public PipelineTasksControl()
        {
            InitializeComponent();
            EditTaksForm = new EditTaksForm();
        }


        public void SetEnabledActions()
        {
            bool single = SelectedItems.Count == 1;
            bool multi = SelectedItems.Count > 1;
            bool any = single || multi;
            bool invalid = SelectedItem is InvalidTypeTask;

            newToolStripMenuItem.Enabled = ButtonNew.Enabled = !multi;
            copyToolStripMenuItem.Enabled = ButtonCopy.Enabled = single && !invalid;
            editToolStripMenuItem.Enabled = ButtonEdit.Enabled = single;
            removeToolStripMenuItem.Enabled = ButtonRemove.Enabled = any;
            enabledToolStripMenuItem.Enabled = ButtonEnabled.Enabled = any;

            upToolStripMenuItem.Enabled = ButtonUp.Enabled = any;
            downToolStripMenuItem.Enabled = ButtonDown.Enabled = any;

            leftToolStripMenuItem.Enabled = ButtonLeft.Enabled = any;
            rightToolStripMenuItem.Enabled = ButtonRight.Enabled = any;
        }

        public void InvalidateItems(bool save = true)
        {
            if (Pipeline == null)
            {
                ListBox.UpdateItems(null);
            }
            else
            {
                ListBox.UpdateItems(Pipeline.Tasks);
            }
            SetEnabledActions();

            //if (save)
            //    Pipeline.Owner.Save();
        }


        private void NewClick(object sender, EventArgs e)
        {
            var task = new NopTask() { Pipeline = Pipeline };

            Pipeline.Tasks.InsertAfter(SelectedItem, task);
            InvalidateItems();
            SelectedItem = null;
            SelectedItem = task;

            /*
            var result = EditTaksForm.ShowDialog(this, Pipeline);
            if (result == DialogResult.OK)
            {
                Pipeline.Tasks.InsertAfter(SelectedItem, EditTaksForm.Task);
                InvalidateItems();
                SelectedItem = EditTaksForm.Task;
                SelectedItem.Pipeline = Pipeline;
            }
            */
        }


        private void EditClick(object sender, EventArgs e)
        {
            var result = EditTaksForm.ShowDialog(this, Pipeline, SelectedItem);
            if (result == DialogResult.OK)
            {
                EditTaksForm.Task.Pipeline = Pipeline;
                int idx = Pipeline.Tasks.IndexOf(SelectedItem);
                Pipeline.Tasks[idx] = EditTaksForm.Task;
                InvalidateItems();
            }
        }

        private void EnabledClick(object sender, EventArgs e)
        {
            foreach (var task in SelectedItems)
            {
                task.Enabled = !task.Enabled;
            }
            InvalidateItems();
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            foreach (var task in SelectedItems)
            {
                Pipeline.Tasks.Remove(task);
            }
            InvalidateItems();
        }

        private void CopyClick(object sender, EventArgs e)
        {
            var clone = SelectedItem.Clone();
            clone.Pipeline = Pipeline;
            Pipeline.Tasks.InsertAfter(SelectedItem, clone);
            InvalidateItems();
        }

        private void UpClick(object sender, EventArgs e)
        {
            var selected = SelectedItems;
            foreach (var task in selected)
            {
                Pipeline.Tasks.UpItem(task);
            }
            InvalidateItems();
            SelectedItems = selected;
        }

        private void DownClick(object sender, EventArgs e)
        {
            var selected = SelectedItems;
            selected.Reverse();
            foreach (var task in selected)
            {
                Pipeline.Tasks.DownItem(task);
            }
            InvalidateItems();
            SelectedItems = selected;
        }

        private void LeftClick(object sender, EventArgs e)
        {
            var selected = SelectedItems;
            foreach (var task in selected)
            {
                if (task.Scope > 0)
                {
                    task.Scope -= 1;
                }
            }
            InvalidateItems();
            SelectedItems = selected;
        }

        private void RightClick(object sender, EventArgs e)
        {
            var selected = SelectedItems;
            selected.Reverse();
            foreach (var task in selected)
            {
                task.Scope += 1;
            }
            InvalidateItems();
            SelectedItems = selected;
        }

        private void ListBoxDoubleClick(object sender, EventArgs e)
        {
            if (SelectedItem == null)
                return;
            EditClick(sender, e);
        }

        private void ListBoxIndexChanged(object sender, EventArgs e)
        {
            SetEnabledActions();
        }
    }
}
