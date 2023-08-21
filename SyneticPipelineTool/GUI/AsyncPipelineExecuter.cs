using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI;

internal class AsyncPipelineExecuter
{
    public Runtime Runtime;

    //public Stack<Pipeline> CallStack;

    public Task task;

    public bool Running { get; private set; }

    public AsyncPipelineExecuter()
    {
        Runtime = new Runtime();
        //CallStack = new Stack<Pipeline>();
    }

    public void Execute(Pipeline pipeline)
    {
        if (Running)
        {
            throw new InvalidOperationException();
        }

        Running = true;
        Runtime.Clear();

        task = Task.Run(() =>
        {
            try
            {
                Runtime.Call(pipeline);
            }
            catch (Exception ex)
            {
                Running = false;

                string stackTrace = Runtime.StackTrace;
                string message = $"{ex.Message}\n\n{stackTrace}";
                Console.WriteLine(message);
                MessageBox.Show(message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Running = false;
        });

    }
}
