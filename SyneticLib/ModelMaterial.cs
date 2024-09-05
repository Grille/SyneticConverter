using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;
public class ModelMaterial : Material
{
    public Vector3 Diffuse;
    public Vector3 Ambient;
    public Vector3 Specular;
    public Vector3 Reflect;
    public Vector3 Specular2;
    public Vector3 XDiffuse;
    public Vector3 XSpecular;

    public float Transparency;

    public ModelMaterial() : base(3)
    {
    }
}
