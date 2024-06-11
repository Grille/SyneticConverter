using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.Math3D;
public static class RayCaster
{
    const float Epsilon = 0.001f;

    public struct RayIntersectionResult
    {
        public readonly bool IsHit;
        public readonly float Distance;
        public readonly int Index;

        public RayIntersectionResult(bool isHit, float distance, int index = -1)
        {
            IsHit = isHit;
            Distance = distance;
            Index = index;
        }

        public Vector3 GetIntersection(Ray ray)
        {
            return ray.Origin + ray.Direction * Distance;
        }

        public void ApplyIfCloserHit(RayIntersectionResult ray)
        {
            if (ray.IsHit && ray.Distance < Distance) {
                this = ray;
            }
        }
    }

    record struct Triangle(Vector3 Vertex0, Vector3 Vertex1, Vector3 Vertex2);

    public static readonly RayIntersectionResult NoHit = new RayIntersectionResult(false, float.PositiveInfinity);

    public static bool RayIntersectsBoundings(Ray ray, BoundingBox boundings)
    {
        var rayOrigin = ray.Origin;
        var rayDirection = ray.Direction;
        var boxMin = boundings.Start;
        var boxMax = boundings.End;
        var tMin = 0.0f;
        var tMax = float.MaxValue;

        for (var i = 0; i < 3; i++)
        {
            if (Math.Abs(rayDirection[i]) < Epsilon)
            {
                // Ray is parallel to the slab. No hit if origin not within slab.
                if (rayOrigin[i] < boxMin[i] || rayOrigin[i] > boxMax[i])
                {
                    return false;
                }
            }
            else
            {
                // Compute intersection t value of ray with near and far plane of slab
                var ood = 1.0f / rayDirection[i];
                var t1 = (boxMin[i] - rayOrigin[i]) * ood;
                var t2 = (boxMax[i] - rayOrigin[i]) * ood;

                // Make t1 be intersection with near plane, t2 with far plane
                if (t1 > t2)
                {
                    var temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                // Compute the intersection of slab intersection intervals
                tMin = Math.Max(tMin, t1);
                tMax = Math.Min(tMax, t2);

                // Exit with no collision as soon as slab intersection becomes empty
                if (tMin > tMax)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static RayIntersectionResult RayIntersectsModel(Ray ray, Model model)
    {
        return RayIntersectsMesh(ray, model.MeshSection);
    }

    public static RayIntersectionResult RayIntersectsMesh(Ray ray, MeshSegment mesh, bool checkBoundings = true, bool returnFirst = false)
    {
        if (checkBoundings && !RayIntersectsBoundings(ray, mesh.BoundingBox))
        {
            return NoHit;
        }

        var indices = mesh.Indices;
        var vertices = mesh.Vertices;

        var offset = mesh.Offset;

        var closest = NoHit;

        for (var i = 0; i < indices.Length; i++)
        {
            var triangle = new Triangle(
                vertices[indices[i].X + offset].Position,
                vertices[indices[i].Y + offset].Position,
                vertices[indices[i].Z + offset].Position
            );

            var result = RayIntersectsTriangle(ray, triangle, indices[i].X + offset);
            if (result.IsHit && result.Distance < closest.Distance)
            {
                closest = result;
                if (returnFirst)
                {
                    break;
                }
            }
        }

        return closest;
    }

    private static RayIntersectionResult RayIntersectsTriangle(Ray ray, Triangle triangle, int index)
    {
        var edge1 = triangle.Vertex1 - triangle.Vertex0;
        var edge2 = triangle.Vertex2 - triangle.Vertex0;

        var h = Vector3.Cross(ray.Direction, edge2);
        var a = Vector3.Dot(edge1, h);

        if (a > -Epsilon && a < Epsilon)
            return NoHit; // This ray is parallel to this triangle.

        var f = 1.0f / a;
        var s = ray.Origin - triangle.Vertex0;
        var u = f * Vector3.Dot(s, h);

        if (u < 0.0f || u > 1.0f)
            return NoHit;

        var q = Vector3.Cross(s, edge1);
        var v = f * Vector3.Dot(ray.Direction, q);

        if (v < 0.0f || u + v > 1.0f)
            return NoHit;

        // At this stage we can compute t to find out where the intersection point is on the line.
        var t = f * Vector3.Dot(edge2, q);

        if (t > Epsilon) // ray intersection
            return new(true, t, index);

        else // This means that there is a line intersection but not a ray intersection.
            return NoHit;
    }


    public static RayIntersectionResult RayIntersectsGround(Ray ray)
    {
        var rayPosition = ray.Origin;
        var rayDirection = ray.Direction;

        var t = -rayPosition.Y / rayDirection.Y;

        return new RayIntersectionResult(t > 0, t);
    }
}
