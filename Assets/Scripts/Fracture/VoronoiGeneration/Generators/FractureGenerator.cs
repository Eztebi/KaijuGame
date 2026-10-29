using EsteboFracture;
using System.Collections.Generic;
using UnityEngine;

namespace EsteboFracture
{
    public static class FractureGenerator
    {
        public static Mesh[] GenerateFracture(EFractureType type, VoronoiCell[] cells, Mesh mesh)
        {
            return type switch
            {
                EFractureType.Slicer => SliceFracture(mesh, cells),
                _ => System.Array.Empty<Mesh>()
            };
        }

        public static void VoronoiFracture()
        {

        }
        public static Mesh[] SliceFracture(Mesh mesh, VoronoiCell[] cells)
        {
            List<Plane> planes = new List<Plane>();
            for (int i = 0; i < cells.Length; i++)
            {

                for (int j = 0; j <= cells.Length-1; j++)
                {
                    if (i == j) continue;
                    Vector3 point = (cells[i].GetSite.Position + cells[j].GetSite.Position) / 2;
                    Vector3 normal = (cells[i].GetSite.Position - cells[j].GetSite.Position).normalized;
                    Plane plane = new Plane(normal, point);
                    planes.Add(plane);
                }
            }
            Debug.Log(planes.Count);
            Mesh[] meshes = new Mesh[planes.Count];
            return meshes;
        }
    }
}
