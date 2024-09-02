using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.PipelineTool;

namespace SyneticPipelineTool.Tasks;

public abstract class ConverterTask<T> : PipelineTask
{
    readonly Dictionary<string, T> _converters;

    readonly string[] _keys;

    public ConverterTask(IEnumerable<KeyValuePair<string,T>> collection)
    {
        _converters = new Dictionary<string, T>();
    }

    protected void Register(string key, T value)
    {
        _converters.Add(key, value);
    }

}
