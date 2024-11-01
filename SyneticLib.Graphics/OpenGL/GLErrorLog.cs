using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics.OpenGL;

public static class GLErrorLog
{
    private static readonly List<CaughtObject> _caughtFinalizedObjects;
    public static readonly IReadOnlyList<CaughtObject> CaughtFinalizedObjects;

    public static Action<CaughtObject>? CaughtFinalizedObject;

    static GLErrorLog()
    {
        _caughtFinalizedObjects = new List<CaughtObject>();
        CaughtFinalizedObjects = _caughtFinalizedObjects;
    }

    internal static void LogFinalizedObject(GLObject obj)
    {
        var entry = new CaughtObject(DateTime.Now, obj.GetType().Name);
        _caughtFinalizedObjects.Add(entry);
        CaughtFinalizedObject?.Invoke(entry);
    }

    public record struct CaughtObject(DateTime Timestamp, string Obj);
}
