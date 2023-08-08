using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;

namespace SyneticPipelineTool;

internal class AsyncPipelineExecuter
{
    public Stack<Pipeline> CallStack;

    public Task task;

    public bool Running { get; private set; }

    public AsyncPipelineExecuter()
    {
        CallStack = new Stack<Pipeline>();
    }

    public string StackTrace
    {
        get
        {
            var list = CallStack.ToList();
            list.Reverse();
            string stackTrace = "";
            foreach (var item in list)
            {
                stackTrace += $"Pipeline: {item.Name}, Line: {item.TaskPosition + 1}\n";
            }
            return stackTrace;
        }
    }

    public void Execute(Pipeline pipeline)
    {
        if (Running)
        {
            throw new InvalidOperationException();
        }

        Running = true;
        CallStack.Clear();

        task = Task.Run(() =>
        {
            var stack = new Stack<Pipeline>();
            try
            {
                pipeline.Execute(CallStack);
            }
            catch (Exception ex)
            {
                Running = false;

                var list = CallStack.ToList();
                list.Reverse();
                string stackTrace = "";
                foreach (var item in list)
                {
                    stackTrace += $"Pipeline: {item.Name}, Line: {item.TaskPosition + 1}\n";
                }
                string message = $"{ex.Message}\n\n{stackTrace}";
                Console.WriteLine(message);
                MessageBox.Show(message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Running = false;
        });

    }
}
