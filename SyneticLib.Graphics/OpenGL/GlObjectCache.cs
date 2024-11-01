using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Xml.Linq;
using SyneticLib.Graphics.Materials;
using SyneticLib.World;

namespace SyneticLib.Graphics.OpenGL;

public class GlObjectCache<TKey, TValue> : IDisposable, IReadOnlyCollection<TValue> where TKey : notnull where TValue : GLObject
{
    Func<TKey, TValue>? _factory;
    Dictionary<TKey, TValue>? _dict;
    TValue[]? _array;

    bool _disposed;

    public bool IsCoupled { get; private set; }

    public int Count => IsCoupled ? _dict!.Count : _array!.Length;

    public GlObjectCache(Func<TKey, TValue> factory)
    {
        IsCoupled = true;
        _factory = factory;
        _dict = new();
    }

    [MemberNotNull(nameof(_factory), nameof(_dict))]
    void AssertIsCoupled()
    {
        if (!IsCoupled)
        {
            throw new InvalidOperationException();
        }

        if (_factory == null || _dict == null)
        {
            throw new NullReferenceException();
        }
    }


    public TValue GetGlObject(TKey model)
    {
        AssertIsCoupled();

        if (_dict.TryGetValue(model, out var value))
        {
            return value;
        }
        var result = _factory(model);
        _dict.Add(model, result);
        return result;
    }

    /// <summary>
    /// Removes all key objects so that they can be collected.
    /// After this you can only enumerate or dispose the existing <see cref="GLObject"/>’s, any other operation will fail.
    /// </summary>
    public void Uncouple()
    {
        AssertIsCoupled();

        IsCoupled = false;
        _factory = null;

        _array = _dict.Values.ToArray();

        _dict = null;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (var item in this)
        {
            item.Dispose();
        }

        IsCoupled = false;
        _factory = null;
        _dict = null;
        _array = null;
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        return IsCoupled ? _dict!.Values.GetEnumerator() : ((IEnumerable<TValue>)_array!).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class GlObjectCacheGroup : IDisposable
{
    public GlObjectCache<Texture, TextureBuffer> Textures { get; }
    public GlObjectCache<Material, MaterialUniforms> Materials { get; }
    public GlObjectCache<IndexedMesh, MeshBuffer> Meshes { get; }

    public GlObjectCacheGroup()
    {
        Textures = new(Factory);
        Materials = new(Factory);
        Meshes = new(Factory);
    }

    TextureBuffer Factory(Texture key)
    {
        return new TextureBuffer(key);
    }

    MeshBuffer Factory(IndexedMesh key)
    {
        return new MeshBuffer(key);
    }

    MaterialUniforms Factory(Material key) => key switch
    {
        ModelMaterial mMat => new ModelMaterialUniforms(mMat, Textures),
        TerrainMaterial tMat => new TerrainMaterialUniforms(tMat, Textures),
        _ => throw new NotImplementedException(),
    };

    public void Uncouple()
    {
        Textures.Uncouple();
        Materials.Uncouple();
        Meshes.Uncouple();
    }

    public void Dispose()
    {
        Textures.Dispose();
        Materials.Dispose();
        Meshes.Dispose();
    }
}
