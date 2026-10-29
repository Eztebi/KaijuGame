using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Drawing;

namespace EsteboFracture
{
    public static class PointGenerator
    {
        public static VoronoiCell[] GeneratePoints(EPointGenerationType type, int totalPoints, Mesh mesh)
        {
            return type switch
            {
                EPointGenerationType.Random => RandomPointGeneration(totalPoints, mesh),
                _ => System.Array.Empty<VoronoiCell>()
            };
        }

        public static VoronoiCell[] RandomPointGeneration(int points, Mesh mesh)
        {
            VoronoiCell[] cellArray = new VoronoiCell[points];
            Bounds bounds = mesh.bounds;

            for (int i = 0; i < points; i++)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                Point point = new Point(randomPos);
                cellArray[i] = new VoronoiCell(point);
            }

            return cellArray;
        }
    }

}
