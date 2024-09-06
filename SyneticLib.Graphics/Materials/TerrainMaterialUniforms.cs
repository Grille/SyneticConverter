using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.World;

namespace SyneticLib.Graphics.Materials;

public class TerrainMaterialUniforms : MaterialUniforms
{
    public readonly TextureTransform UVMatrix0;
    public readonly TextureTransform UVMatrix1;
    public readonly TextureTransform UVMatrix2;
            
    public readonly int Mode0;
    public readonly int Mode1;

    public TerrainMaterialUniforms(TerrainMaterial material, GlObjectCache<Texture, TextureBuffer> textures) : base(material, textures)
    {
        UVMatrix0 = material.Matrix0;
        UVMatrix1 = material.Matrix1;
        UVMatrix2 = material.Matrix2;

        Mode0 = material.Layer0.Mode;
        Mode1 = material.Layer1.Mode;
    }

    protected override void OnBind()
    {
        BindEnabledTextures();



        if (GLContext.UsedProgram is TerrainMaterialProgram material)
        {
            material.ApplyUniforms(this);
        }
        //throw new NotImplementedException();
    }

    protected override void OnDelete()
    {
        //throw new NotImplementedException();
    }
}
