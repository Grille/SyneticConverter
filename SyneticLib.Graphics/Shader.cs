using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;
using SyneticLib.LowLevel;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics
{
    internal class Shader : GLObject
    {
        public int VertexID { get; }
        public int FragmentID { get; }

        private Shader(Context ctx) : base(ctx) {
            VertexID = GL.CreateShader(ShaderType.VertexShader);
            FragmentID = GL.CreateShader(ShaderType.FragmentShader);
        }

        public Shader(Context ctx, MaterialShaderType type) : this(ctx)
        {
            var files = GLSLSources.LoadInternalShaderFiles(type);
            Compile(files.Vert, files.Frag);
        }

        public Shader(Context ctx, string vertex, string fragment) : this(ctx) {
            Compile(vertex, fragment);
        }

        private void Compile(string vertex, string fragment)
        {
            GL.ShaderSource(VertexID, vertex);
            GL.CompileShader(VertexID);
            GL.GetShaderInfoLog(VertexID, out string vertlog);
            if (vertlog != "")
                throw new ArgumentException(vertlog);

            GL.ShaderSource(FragmentID, fragment);
            GL.CompileShader(FragmentID);
            GL.GetShaderInfoLog(FragmentID, out string idxlog);
            if (idxlog != "")
                throw new ArgumentException(idxlog);
        }

        protected override void OnBind()
        {
            throw new NotImplementedException();
        }

        protected override void OnDelete()
        {
            GL.DeleteShader(VertexID);
            GL.DeleteShader(FragmentID);
        }
    }
}
