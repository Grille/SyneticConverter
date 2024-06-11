using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using Grille.PipelineTool.WinForms.Tree;
using Grille.PipelineTool;
using System.Reflection;

namespace SyneticPipelineTool;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        AssemblyTaskTypeTree.LoadAssembly(typeof(Program).Assembly);
        AssemblyTaskTypeTree.Initialize();

        ApplicationConfiguration.Initialize();
        Application.Run(new SynPipelineToolForm());
    }
}