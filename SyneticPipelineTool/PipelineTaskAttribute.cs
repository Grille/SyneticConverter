using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

internal class PipelineTaskAttribute : Attribute
{
    public string Name { get; init; }
}
