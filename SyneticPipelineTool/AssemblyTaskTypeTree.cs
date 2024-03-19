using SyneticPipelineTool.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SyneticPipelineTool;

public static class AssemblyTaskTypeTree
{
    public class TypeInfo: TreeNode
    {
        public Type Type { get; }

        public string[] NamespacePath { get; }

        public string Key { get; }

        public string Description { get; }

        public TypeInfo(Type type, string key, string description)
        {
            Type = type;
            Key = key;
            Description = description;

            var split = key.Split('/');
            var name = split[split.Length - 1];

            if (split.Length < 1)
                throw new Exception();
            var path = new string[split.Length - 1];
            Array.Copy(split, path, path.Length);

            Name = name;
            NamespacePath = path;

            Text = Name;
        }

        public TypeInfo(Type type, string key) :
            this(type, key, "")
        { }

        public TypeInfo(Type type, PipelineTaskAttribute attribute) :
            this(type, attribute.Key, attribute.Description)
        { }

        public override string ToString() => Key;

    }

    public class Namespace : TreeNode
    {
        public Dictionary<string, TypeInfo> Values { get; set; } = new();
        public Dictionary<string, Namespace> Groups { get; set; } = new();

        public Namespace(string name)
        {
            Name = name;
            Text = name;
        }

        public void Init()
        {
            foreach (var group in Groups.Values)
            {
                Nodes.Add(group);

                group.Init();
            }

            foreach (var type in Values.Values)
            {
                Nodes.Add(type);
            }
        }
    }

    static Namespace root;

    public static Namespace Root
    {
        get
        {
            if (root == null)
                Init();

            return root;
        }
    }

    static List<TypeInfo> types;

    static public void ApplyTo(TreeNodeCollection collection)
    {
        foreach (var node in Root.Nodes)
        {
            collection.Add((TreeNode)node);
        }
    }

    public static TypeInfo GetTypeInfo(Type type)
    {
        foreach (var typeInfo in types)
        {
            if (typeInfo.Type == type) return typeInfo;
        }
        throw new NotImplementedException();
    }

    static void Init()
    {
        Console.WriteLine("Init type tree...");

        types = new List<TypeInfo>
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

            types.Add(new TypeInfo(type, attr));
        }

        types.Sort((a, b) => a.Key.CompareTo(b.Key));

        root = new Namespace("ROOT");

        foreach (var typeInfo in types)
        {
            string[] path = typeInfo.NamespacePath;
            var currentNode = root;

            foreach (string segment in path)
            {
                if (!currentNode.Groups.TryGetValue(segment, out var childNode))
                {
                    childNode = new Namespace(segment);
                    currentNode.Groups.Add(segment, childNode);
                }

                currentNode = childNode;
            }

            if (currentNode.Values.ContainsKey(typeInfo.Name))
            {
                throw new Exception();
            }
            currentNode.Values[typeInfo.Name] = typeInfo;
        }

        root.Init();

        Console.WriteLine($"{types.Count} found.");
    }
}
