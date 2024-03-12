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
using System.Xml.Linq;

namespace SyneticPipelineTool;

public static class AssemblyTaskTypeTree
{
    public class TypeInfo
    {

        public Type Type { get; }

        public string Name { get; }

        public string[] Path { get; }

        public string Key { get; }

        public string Description { get; }

        public TypeInfo(Type type, string key, string description)
        {
            var split = key.Split('/');

            string name = split[split.Length - 1];

            string[] path = new string[split.Length];
            Array.Copy(split, path, path.Length);

            Type = type;
            Name = string.IsNullOrEmpty(name) ? type.Name : name;
            Path = path;
            Key = key;
            Description = description;
        }

        public TypeInfo(Type type, string key) :
            this(type, key, "")
        { }

        public TypeInfo(Type type, PipelineTaskAttribute attribute) :
            this(type, attribute.Key, attribute.Description)
        { }

        public override string ToString() => Key;

    }

    public class TreeNode
    {
        public TypeInfo Value { get; set; }
        public Dictionary<string, TreeNode> Children { get; set; } = new();
    }

    public class GroupNode
    {

    }

    static TreeNode root;

    public static TreeNode Root
    {
        get
        {
            if (root == null)
                Init();

            return root;
        }
    }

    static void Init()
    {
        var types = new List<TypeInfo>
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

        root = new TreeNode();

        foreach (var obj in types)
        {
            string[] pathSegments = obj.Path;
            TreeNode currentNode = root;

            foreach (string segment in pathSegments)
            {
                if (!currentNode.Children.TryGetValue(segment, out TreeNode childNode))
                {
                    childNode = new TreeNode();
                    currentNode.Children.Add(segment, childNode);
                }

                currentNode = childNode;
            }

            currentNode.Value = obj;
        }

    }
}
