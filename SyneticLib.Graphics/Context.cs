using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;

public class Context
{
    public GLObjectCache<Texture, TextureBuffer> TextureCache { get; }
    public GLObjectCache<Mesh, MeshBuffer> MeshCache { get; }
    public GLObjectCache<Material, MaterialProgram> MaterialCache { get; }

    public Camera Camera { get; internal set; }
    public MeshBuffer BoundMesh { get; internal set; }
    public MaterialProgram BoundProgram { get; internal set; }

    public Context()
    {
        TextureCache = new(tex => new(this, tex));
        MeshCache = new(model => new(this, model));
        MaterialCache = new(model => new(this, model));
    }

    public MeshBuffer Create(Mesh model)
    {
        return MeshCache.Get(model);
    }

    public TextureBuffer Create(Texture model)
    {
        return TextureCache.Get(model);
    }

    public MaterialProgram Create(Material model)
    {
        return MaterialCache.Get(model);
    }

    public void AssertNoError()
    {
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            throw new InvalidOperationException($"{DateTime.Now} {error}");
        }
    }
}
