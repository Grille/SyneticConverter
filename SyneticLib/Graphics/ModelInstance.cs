using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics;
public class ModelInstance
{
    public Model Model; 
    public Matrix4 ModelMatrix;

    public ModelInstance(Model mesh): this(mesh, Matrix4.Identity)
    {

    }

    public ModelInstance(Model mesh, Matrix4 modelMatrix)
    {
        Model = mesh;
        ModelMatrix = modelMatrix;
    }
}
