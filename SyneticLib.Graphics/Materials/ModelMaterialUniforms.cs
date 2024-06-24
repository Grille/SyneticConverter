using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.Materials;

public class ModelMaterialUniforms : MaterialUniforms
{
    public Vector3 Diffuse;

    public ModelMaterialUniforms(ModelMaterial material, GlObjectCache<Texture, TextureBuffer>? textures) : base(material, textures)
    {
        Diffuse = material.Diffuse;
    }

    protected override void OnBind()
    {
        BindEnabledTextures();

        if (GLContext.UsedProgram is ModelMaterialProgram material)
        {
            material.ApplyUniforms(this);
        }
    }

    protected override void OnDelete()
    {
        //throw new NotImplementedException();
    }
}
