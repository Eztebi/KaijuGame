using EsteboFracture;
using System.Collections.Generic;
using UnityEngine;

namespace EsteboFracture
{
    public static class FractureGenerator
    {
        public static List<Mesh> GenerateFracture(EFractureType type, VoronoiCell[] cells, Mesh mesh)
        {
            return type switch
            {
                EFractureType.Slicer => SliceFracture(mesh, cells),
                _ => new List<Mesh>()
            };
        }

        public static void VoronoiFracture()
        {

        }
        public static List<Mesh> SliceFracture(Mesh mesh, VoronoiCell[] cells)
        {
            List<Mesh> meshes = new List<Mesh>();
            List<Plane> planes = new List<Plane>();

            for (int i = 0; i < cells.Length; i++)
            {
                Mesh currentFragment = Object.Instantiate(mesh);

                for (int j = 0; j <= cells.Length-1; j++)
                { 
                    if (i == j) continue;
                    Vector3 point = (cells[i].GetSite.Position + cells[j].GetSite.Position) / 2;
                    Vector3 normal = (cells[j].GetSite.Position - cells[i].GetSite.Position).normalized;

                    Plane plane = new Plane(normal, point);

                    currentFragment = MeshSlicer.Slice(currentFragment, plane);
                    planes.Add(plane);
                }

                meshes.Add(currentFragment);
            } 
            Debug.Log(planes.Count);
            Debug.Log(meshes.Count);
            return meshes;
        }
    }
}
