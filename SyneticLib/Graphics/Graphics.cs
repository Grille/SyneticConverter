using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace SyneticLib.Graphics;
public static class Graphics
{
    static Camera Camera;

    public static void BindCamera(Camera camera)
    {
        Camera = camera;
    }

    public static void BindMaterial(Material material)
    {
        AssertRessource(material);
        AssertGLState(material.GLProgram);
        material.GLProgram.Bind();
    }

    public static void BindMesh(Mesh mesh)
    {
        AssertRessource(mesh);
        AssertGLState(mesh.GLBuffer);
        mesh.GLBuffer.Bind();
    }

    public static void BindTexture(Texture texture)
    {
        AssertRessource(texture);
        AssertGLState(texture.GLBuffer);
        texture.GLBuffer.Bind();
    }

    public static void ClearScreen()
    {
        GL.ClearColor(Color.DarkGray);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public static void DrawModel(ModelInstance instance)
    {
        DrawModel(instance.Model, instance.ModelMatrix);
    }

    public static void DrawModel(Model model, in Matrix4 matrix)
    {
        AssertRessource(model);
        BindMesh(model);

        for (int i = 0; i < model.MaterialRegion.Length; i++)
        {
            var region = model.MaterialRegion[i];

            var material = region.Material;
            var program = material.GLProgram;
            var texture = region.Material.TexSlot0.Texture;


            BindMaterial(material);
            program.SubCameraMatrix(Camera);
            program.SubModelMatrix(matrix);

            BindTexture(texture);

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



    public static void AssertRessource(Ressource ressource)
    {
        if (ressource.DataState != DataState.Loaded)
            throw new ArgumentException(nameof(ressource), "Ressource is not loaded.");
    }

    public static void AssertGLState(GLStateObject obj)
    {
        if (!obj.TryCreate())
            throw new ArgumentException(nameof(obj), "GLStateObject is not ready.");
    }

}
