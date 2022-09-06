using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using static OpenTK.Graphics.OpenGL.GL;

namespace SyneticLib.Graphics;
public static class Graphics
{
    static Camera Camera;

    public static bool DepthTest
    {
        set
        {
            if (value) GL.Enable(EnableCap.DepthTest);
            else GL.Disable(EnableCap.DepthTest);
        }
    }

    public static bool CullFace
    {
        set
        {
            if (value) GL.Enable(EnableCap.CullFace);
            else GL.Disable(EnableCap.CullFace);
        }
    }

    public static void Setup()
    {
        GL.Disable(EnableCap.CullFace);
    }

    public static void BindCamera(Camera camera)
    {
        Camera = camera;
        GL.Enable(EnableCap.DepthTest);
    }

    public static void BindGLStateObject(GLStateObject glObj)
    {
        AssertGLState(glObj);
        glObj.Bind();
    }

    public static void BindMaterial(Material material)
    {
        AssertRessource(material);
        BindGLStateObject(material.GLProgram);
    }

    public static void BindMesh(Mesh mesh)
    {
        AssertRessource(mesh);
        BindGLStateObject(mesh.GLBuffer);
    }

    public static void BindTexture(Texture texture)
    {
        AssertRessource(texture);
        BindGLStateObject(texture.GLBuffer);
    }

    public static void ClearScreen()
    {
        GL.ClearColor(Color.DarkGray);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public static void DrawModel(ModelInstance instance) => DrawModel(instance.Model, instance.ModelMatrix);

    public static void DrawModel(Model model) => DrawModel(model, Matrix4.Identity);

    public static void DrawModel(Model model, in Matrix4 matrix)
    {
        AssertRessource(model);
        BindMesh(model);

        for (int i = 0; i < model.MaterialRegion.Length; i++)
        {
            var region = model.MaterialRegion[i];

            var material = region.Material;
            var program = material.GLProgram;

            BindMaterial(material);
            program.SubCameraMatrix(Camera);
            program.SubModelMatrix(matrix);

            if (material.TexSlot0.Enabled)
                BindTexture(material.TexSlot0.Texture);

            GL.DrawElements(PrimitiveType.Triangles, region.ElementCount * 3, DrawElementsType.UnsignedInt, region.ElementOffset * 3 * 4);
        }


        /*
        var rnd = new Random(1);
        */

        //for (int i = 0; i < mesh.MaterialRegion.Length; i++)
        //{
        //var region = mesh.MaterialRegion[i];
        //GL.Uniform3(colorLoc, new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
        //region.Material.GLShader.Bind();
        //GL.DrawElements(PrimitiveType.Triangles, buffer.ElementCount, DrawElementsType.UnsignedInt, 0 * 3 * 4);

        //}
    }

    public static void DrawTerrain(Terrain terrain)
    {
        AssertRessource(terrain);
        BindMesh(terrain);

        for (int i = 0; i < terrain.MaterialRegion.Length; i++)
        {

            var region = terrain.MaterialRegion[i];

            var material = region.Material;
            var program = material.GLProgram;
            var texture = region.Material.TexSlot0.Texture;


            BindMaterial(material);
            program.SubCameraMatrix(Camera);

            BindTexture(texture);

            GL.DrawElements(PrimitiveType.Triangles, region.ElementCount * 3, DrawElementsType.UnsignedInt, region.ElementOffset * 3 * 4);
        }
    }

    public static void DrawSprite(Sprite sprite)
    {
        AssertRessource(sprite.Texture);
        BindGLStateObject(Sprite.GLBuffer);
        BindGLStateObject(Sprite.GLProgram);

        float aspectTex = ((float)sprite.Texture.Width / (float)sprite.Texture.Height);
        float aspect = Camera.AspectRatio / aspectTex;
        float scale = 0.98f;

        if (aspect > 1.0f)
            scale *= 1.0f / aspect;

        GL.Uniform2(Sprite.GLProgram.UScale, new Vector2(1 * scale, 1 * aspect * scale));

        if (!sprite.Texture.GLBuffer.TryCreate())
            throw new InvalidOperationException("Could not create GL buffer.");

        sprite.Texture.GLBuffer.Bind();

        GL.DrawElements(PrimitiveType.Triangles, Sprite.GLBuffer.ElementCount, DrawElementsType.UnsignedShort, 0 * 3 * 2);
    }



    public static void AssertRessource(Ressource ressource)
    {
        if (ressource.DataState != DataState.Loaded)
            throw new ArgumentException(nameof(ressource), $"{ressource.GetType().Name} is not loaded.");
    }

    public static void AssertGLState(GLStateObject obj)
    {
        if (!obj.TryCreate())
            throw new ArgumentException(nameof(obj), $"{obj.GetType().Name} is not ready.");
    }

}
