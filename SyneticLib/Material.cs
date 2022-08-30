using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;

using SyneticLib.Graphics;

namespace SyneticLib;

public abstract class Material : Ressource
{
    public Texture Texture0;
    public Texture Texture1;
    public Texture Texture2;

    public Matrix4 Transform0;
    public Matrix4 Transform1;
    public Matrix4 Transform2;

    public GLProgram GLProgram;

    protected Material(Ressource parent, string path, PointerType type = PointerType.Virtual) : base(parent, path, type)
    {

    }
}
