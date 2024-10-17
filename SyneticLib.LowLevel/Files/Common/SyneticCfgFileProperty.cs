using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files.Common;

public class SyneticCfgFileProperty
{
    readonly Dictionary<string, string> _dict;

    public string Key { get; }

    public SyneticCfgFileProperty(Dictionary<string, string> dict, string key)
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

    public virtual string? Value
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
}

public abstract class SyneticCfgFileProperty<T> : SyneticCfgFileProperty
{
    T? _cache;
    bool _cacheValid = false;

    public SyneticCfgFileProperty(Dictionary<string, string> dict, string key) : base(dict, key) { }

    public bool AutoFlush { get; set; } = true;

    public override string? Value
    {
        get => base.Value;
        set
        {
            base.Value = value;
            _cacheValid = false;
        }
    }

    public T? Object
    {
        get
        {
            if (_cacheValid)
            {
                return _cache;
            }
            if (Exists == false)
            {
                return default;
            }
            Pull();
            return _cache;
        }

        set
        {
            _cache = value;
            if (AutoFlush)
            {
                Flush();
            }
        }
    }

    public T GetObject()
    {
        if (TryGetObject(out var obj))
        {
            return obj;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public bool TryGetObject([MaybeNullWhen(false)] out T obj)
    {
        obj = Object;
        return obj != null;
    }

    private void Pull()
    {
        _cache = Deserialize(base.Value!);
        _cacheValid = true;
    }

    public void Flush()
    {
        base.Value = Serialize(_cache!);
    }

    protected abstract string Serialize(T value);

    protected abstract T Deserialize(string value);
}

public abstract class SyneticCfgFileArrayProperty<T> : SyneticCfgFileProperty<T[]>
{
    public SyneticCfgFileArrayProperty(Dictionary<string, string> dict, string key) : base(dict, key) { }

    protected sealed override T[] Deserialize(string value)
    {
        var split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new T[split.Length];
        for (int i = 0; i < split.Length; i++)
        {
            result[i] = DeserializeItem(split[i]);
        }
        return result;
    }

    protected sealed override string Serialize(T[] value)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < value.Length; i++)
        {
            sb.Append("0x");
            sb.Append(SerializeItem(value[i]));
            sb.Append(' ');
        }
        return sb.ToString();
    }

    protected abstract string SerializeItem(T value);

    protected abstract T DeserializeItem(string value);
}