using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics;

public class GLSyn
{
    static float width;
    static float height;
    static float aspectRatio;


    public static void DrawMesh(Mesh mesh)
    {
        MeshMaterialProgram.Default.Bind();

        Matrix4 projectionMatrix;
        Matrix4 modelMatrix;
        Matrix4 viewMatrix;

        modelMatrix = Matrix4.Identity;
        viewMatrix = Matrix4.Identity;
        projectionMatrix = Matrix4.Identity;

        int modelLoc = GL.GetUniformLocation(MeshMaterialProgram.Default.ProgramID, "model");
        GL.UniformMatrix4(modelLoc, false, ref modelMatrix);

        viewMatrix = Matrix4.LookAt(new Vector3(0, 10000, -20000), new Vector3(0, 0, -10000), new Vector3(0, 1, 0));
        int viewLoc = GL.GetUniformLocation(MeshMaterialProgram.Default.ProgramID, "view");
        GL.UniformMatrix4(viewLoc, false, ref viewMatrix);

        projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, aspectRatio, 1.0f, 80000.0f);
        int projectionLoc = GL.GetUniformLocation(MeshMaterialProgram.Default.ProgramID, "projection");
        GL.UniformMatrix4(projectionLoc, false, ref projectionMatrix);

        int colorLoc = GL.GetUniformLocation(MeshMaterialProgram.Default.ProgramID, "uColor");
        GL.Uniform3(colorLoc, new Vector3(1, 1, 1));

        //return;
        GL.Enable(EnableCap.DepthTest);
        //GL.FrontFace(FrontFaceDirection.Ccw);

        var buffer = mesh.GLBuffer;
        buffer.Bind();

        var rnd = new Random(1);

        for (int i = 0; i < mesh.MaterialRegion.Length; i++)
        {


            var region = mesh.MaterialRegion[i];
            GL.Uniform3(colorLoc, new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
            //region.Material.GLShader.Bind();
            GL.DrawElements(PrimitiveType.Triangles, region.Count * 3, DrawElementsType.UnsignedInt, region.Offset * 3 * 4);
        }



    }

}
