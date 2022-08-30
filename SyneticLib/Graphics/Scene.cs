﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace SyneticLib.Graphics;
public class Scene
{
    public readonly List<Sprite> Sprites;
    public readonly List<ModelInstance> Instances;
    public Terrain Terrain;
    public Camera Camera;

    public Mesh Grid;
    public Mesh Skybox;

    public Scene()
    {
        Sprites = new List<Sprite>();
        Instances = new List<ModelInstance>();
        Camera = new OrbitCamera();
    }


    public void ClearScreen()
    {
        GL.ClearColor(Color.DarkGray);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void ClearScene()
    {
        Terrain = null;
        Instances.Clear();
        Sprites.Clear();
    }

    private void AssertRessource(Ressource ressource)
    {
        if (ressource.DataState != DataState.Loaded)
            throw new ArgumentException("ressource", "Ressource is not loaded.");
    }

    private void DrawTerrain(Terrain terrain)
    {
        AssertRessource(terrain);

        if (!terrain.GLBuffer.TryCreate())
            throw new InvalidOperationException("Could not create GL buffer.");

        var material = TerrainMaterial.Default;
        var program = material.GLProgram;
        program.TryCreate();

        program.Bind();
        program.SubCameraMatrix(Camera);

        int colorLoc = GL.GetUniformLocation(program.ProgramID, "uColor");
        GL.Uniform3(colorLoc, new Vector3(1, 1, 1));


        var buffer = terrain.GLBuffer;
        buffer.Bind();
        var rnd = new Random(1);

        /*
        for (int i = 0; i < terrain.MaterialRegion.Length; i++)
        {
            var region = terrain.MaterialRegion[i];
            GL.Uniform3(colorLoc, new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
            //region.Material.GLShader.Bind();
            GL.DrawElements(PrimitiveType.Triangles, region.Count * 3, DrawElementsType.UnsignedInt, region.Offset * 3 * 4);
        }
        */

        for (int i = 0; i < terrain.Chunks.Length; i++)
        {
            var region = terrain.Chunks[i];
            GL.Uniform3(colorLoc, new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
            //region.Material.GLShader.Bind();
            GL.DrawElements(PrimitiveType.Triangles, region.ElementCount * 3, DrawElementsType.UnsignedInt, region.ElementOffset * 3 * 4);
        }
    }

    private void DrawMesh(ModelInstance instance)
    {
        AssertRessource(instance.Model);

        if (!instance.Model.GLBuffer.TryCreate())
            throw new InvalidOperationException("Could not create GL buffer.");

        var mesh = instance.Model;
        var material = ModelMaterial.Default;
        var program = material.GLProgram;
        program.TryCreate();

        program.Bind();
        program.SubCameraMatrix(Camera);
        program.SubModelMatrix(instance.ModelMatrix);

        int colorLoc = GL.GetUniformLocation(program.ProgramID, "uColor");
        GL.Uniform3(colorLoc, new Vector3(1, 1, 1));


        var buffer = mesh.GLBuffer;
        buffer.Bind();
        var rnd = new Random(1);

        //for (int i = 0; i < mesh.MaterialRegion.Length; i++)
        //{
            //var region = mesh.MaterialRegion[i];
            GL.Uniform3(colorLoc, new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
            //region.Material.GLShader.Bind();
            GL.DrawElements(PrimitiveType.Triangles, buffer.ElementCount, DrawElementsType.UnsignedInt, 0 * 3 * 4);
        //}
    }

    private void DrawSprite(Sprite sprite)
    {
        AssertRessource(sprite.Texture);

        if (!sprite.Texture.GLBuffer.TryCreate())
            return;
    }

    public void Render()
    {
        GL.Viewport(0, 0, (int)Camera.ScreenSize.X, (int)Camera.ScreenSize.Y);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        Camera.CreatePerspective();
        Camera.CreateView();

        if (Terrain != null)
            DrawTerrain(Terrain);

        foreach (var instance in Instances)
            DrawMesh(instance);

        GL.Disable(EnableCap.DepthTest);

        foreach (Sprite sprite in Sprites)
            DrawSprite(sprite);
    }
}
