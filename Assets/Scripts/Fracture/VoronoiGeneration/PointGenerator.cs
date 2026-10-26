using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Drawing;

namespace EsteboFracture
{
    public class PointGenerator : MonoBehaviour
    {
        public static void GeneratePoints(EPointGenerationType type, VoronoiCell[] cellArray, int totalPoints, Mesh mesh)
        {
            VoronoiCell[] points = type switch
            {
                EPointGenerationType.Random => RandomPointGeneration(cellArray, totalPoints, mesh),
                _ => System.Array.Empty<VoronoiCell>() 
            };
        }
        public static VoronoiCell[] RandomPointGeneration(VoronoiCell[] cellArray, int points, Mesh mesh)
        {
            for (int i = 0; i< points; i++)
            {

                Point point = new Point(new Vector3(Random.Range(mesh.bounds.min.x, mesh.bounds.max.x),
                    Random.Range(mesh.bounds.min.y, mesh.bounds.max.y),
                    Random.Range(mesh.bounds.min.z, mesh.bounds.max.z)));
                VoronoiCell randomPos = new VoronoiCell(point);
                cellArray[i] = randomPos;
            }
            return cellArray;
        }


    }
    
}
