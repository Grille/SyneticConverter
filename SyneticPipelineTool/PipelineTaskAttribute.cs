﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public class PipelineTaskAttribute : Attribute
{
    public string Key { get; set; }
    public string Description { get; set; }
}
