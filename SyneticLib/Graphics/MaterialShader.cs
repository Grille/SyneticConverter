using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public class MaterialShader : IDisposable
{
    internal Material Material;
    internal int VertexId;
    internal int FragmentId;
    internal int ProgramId;

    public bool IsInitialized;


    public MaterialShader(Material target)
    {
        Material = target;
    }

    static MaterialShader()
    {
        Default = new MaterialShader(null);
        Default.Initialize();
    }

    public static MaterialShader Default;

    public void Bind()
    {
        GL.UseProgram(ProgramId);
    }

    public void Initialize()
    {
        VertexId = GL.CreateShader(ShaderType.VertexShader);
        FragmentId = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(VertexId, GLSLSrc.BasicVertexSrc);
        GL.CompileShader(VertexId);
        GL.GetShaderInfoLog(VertexId, out string vertlog);

        GL.ShaderSource(FragmentId, GLSLSrc.BasicFragmentSrc);
        GL.CompileShader(FragmentId);
        GL.GetShaderInfoLog(FragmentId, out string idxlog);

        ProgramId = GL.CreateProgram();
        GL.AttachShader(ProgramId, VertexId);
        GL.AttachShader(ProgramId, FragmentId);
        GL.LinkProgram(ProgramId);

        GL.DeleteShader(VertexId);
        GL.DeleteShader(FragmentId);

        IsInitialized = true;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsInitialized)
        {
            GL.DeleteProgram(ProgramId);
            IsInitialized = false;
        }

    }

    ~MaterialShader()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
