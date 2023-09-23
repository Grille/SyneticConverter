using SyneticPipelineTool.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public static class AssemblyTaskTypeTable
{
    public class Entry
    {
        public Type Type { get; }

        public string Name { get; }

        public string[] Path { get; }

        public string Key { get; }

        public string Description { get; }

        public Entry(Type type, string key, string description)
        {
            var split = key.Split('/');

            string name = split[split.Length - 1];

            string[] path = new string[split.Length - 1];
            Array.Copy(split, path, path.Length);

            Type = type;
            Name = string.IsNullOrEmpty(name) ? type.Name : name;
            Path = path;
            Key = key;
            Description = description;
        }

        public Entry(Type type, string key) :
            this(type, key, "")
        { }

        public Entry(Type type, PipelineTaskAttribute attribute) :
            this(type, attribute.Key, attribute.Description)
        { }

        public override string ToString() => Key;
    }

    static Entry[] types;

    public static Entry[] Types
    {
        get
        {
            if (types == null)
                Init();

            return types;
        }
    }

    static void Init()
    {
        var types = new List<Entry>
        {
            new(typeof(NopTask), "Comment"),
        };

        var asm = Assembly.GetExecutingAssembly();
        var asmTypes = asm.GetTypes();

        foreach (var type in asmTypes)
        {
            var attr = type.GetCustomAttribute<PipelineTaskAttribute>();
            if (attr == null)
                continue;

            types.Add(new Entry(type, attr));
        }

        types.Sort((a, b) => a.Key.CompareTo(b.Key));

        AssemblyTaskTypeTable.types = types.ToArray();
    }
}
