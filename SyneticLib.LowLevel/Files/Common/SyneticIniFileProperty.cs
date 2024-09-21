using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files.Common;

public class SyneticIniFileProperty
{
    readonly Dictionary<string, string> _dict;

    public string Key { get; }

    public SyneticIniFileProperty(Dictionary<string, string> dict, string key)
    {
        _dict = dict;
        Key = key;
    }

    public bool Exists
    {
        [MemberNotNullWhen(true, nameof(Value))]
        get
        {
            return _dict.ContainsKey(Key);
        }
        set
        {
            if (value)
            {
                if (!_dict.ContainsKey(Key))
                {
                    _dict[Key] = string.Empty;
                }
            }
            else
            {
                _dict.Remove(Key);
            }
        }
    }

    public string? Value
    {
        get => Exists ? _dict[Key] : null;
        set
        {
            if (value == null)
            {
                Exists = false;
            }
            else
            {
                _dict[Key] = value;
            }
        }
    }

    public string String
    {
        get => Value!.Trim(new char[] { ' ', '"' });
        set => Value = $"\"{value}\"";
    }

    public string[] StringArray
    {
        get
        {
            var colorSet = Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = new string[colorSet.Length];
            for (int i = 0; i < colorSet.Length; i++)
            {
                result[i] = colorSet[i].Substring(1, colorSet[i].Length - 2);
            }
            return result;
        }
        set
        {
            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                sb.Append('"');
                sb.Append(value[i]);
                sb.Append('"');
                sb.Append(' ');
            }
        }
    }

    public float Single
    {
        get => float.Parse(Value, CultureInfo.InvariantCulture);
        set => value.ToString("n6");
    }

    public BgraColor[] ColorArray
    {
        get => Unsafe.As<BgraColor[]>(Hex24Array);
        set => Hex24Array = Unsafe.As<int[]>(value);
    }

    public int[] Hex24Array
    {
        get
        {
            var split = Value!.Split(' ');
            var result = new int[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                result[i] = int.Parse(split[i].Substring(2), NumberStyles.HexNumber);
            }
            return result;
        }
        set
        {
            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                sb.Append("0x");
                sb.Append((value[i] & 0x00ffffff).ToString("x6"));
                sb.Append(' ');
            }
            Value = sb.ToString();
        }
    }
}
