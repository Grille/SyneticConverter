using GGL.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticPipelineTool.Tasks;

internal class InvalidTypeTask : PipelineTask
{
    public string AssemblyQualifiedName;
    public int ParametersCount;

    public InvalidTypeTask(string assemblyQualifiedName, int parametersCount) : base(false)
    {
        AssemblyQualifiedName = assemblyQualifiedName;
        ParametersCount = parametersCount;

        Init();
    }

    protected override void OnInit()
    {
        for (int i = 0; i < ParametersCount; i++)
        {
            Parameters.Def(ParameterTypes.String, $"U{i}");
        }
    }

    protected override void OnExecute()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        string name = AssemblyQualifiedName.Split(',', 2)[0];
        var values = new List<string>();
        foreach ( var para in Parameters)
        {
            values.Add(para.Value);
        }
        string args = string.Join(", ", values.ToArray());
        
        return $"!{name} ({args})";
    }
}
