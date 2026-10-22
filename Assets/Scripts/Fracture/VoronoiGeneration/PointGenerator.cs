using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Drawing;

namespace EsteboFracture
{
    public class PointGenerator : MonoBehaviour
    {
        VoronoiCell[] cellArray;
        void GeneratePoints(EPointGenerationType type, int totalPoints, Mesh mesh)
        {
            VoronoiCell[] points = type switch
            {
                EPointGenerationType.Random => RandomPointGeneration(totalPoints, mesh),
                _ => System.Array.Empty<VoronoiCell>() 
            };
        }
        VoronoiCell[] RandomPointGeneration(int points, Mesh mesh)
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

        void OnDrawGizmos()
        {

            for(int i = 0; i<cellArray.Length; i++)
            {
                Gizmos.color = UnityEngine.Color.yellow;
                Gizmos.DrawSphere(cellArray[i].GetSite.Position, 1);
            }
        }
    }
    
}
