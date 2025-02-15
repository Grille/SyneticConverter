using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Math3D;
using SyneticLib.World;


namespace SyneticLib;
public class Scene
{
    public Camera Camera { get; set; }

    public Scenario? Scenario { get; set; }

    public Scene()
    {
        Camera = new FreeCamera();
    }

    public void CastMouseRay()
    {
        var closest = RayCaster.NoHit;
        var ray = Camera.CastMouseRay();

        if (Scenario != null)
        {
            foreach (var chunk in Scenario.EnumerateChunks())
            {
                closest.ApplyIfCloserHit(RayCaster.RayIntersectsModel(ray, chunk.Terrain));
            }
        }

        if (!closest.IsHit)
        {
            closest = RayCaster.RayIntersectsGround(ray);
        }

        var pos = closest.GetIntersection(ray);
        var mat = Matrix4.CreateTranslation(pos);
    }
}
