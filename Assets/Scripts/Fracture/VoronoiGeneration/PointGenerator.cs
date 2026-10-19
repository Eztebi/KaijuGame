using Unity.VisualScripting;
using UnityEngine;

namespace EsteboFracture
{
    public class PointGenerator : MonoBehaviour
    {
        Vector3[] pointArray;
        void GeneratePoints(EPointGenerationType type, int totalPoints, Mesh mesh)
        {
            Vector3[] points = type switch
            {
                EPointGenerationType.Random => RandomPointGeneration(totalPoints, mesh),
                _ => System.Array.Empty<Vector3>() 
            };
        }
        Vector3[] RandomPointGeneration(int points, Mesh mesh)
        {
            for (int i = 0; i< points; i++)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(mesh.bounds.min.x, mesh.bounds.max.x),
                    Random.Range(mesh.bounds.min.y, mesh.bounds.max.y),
                    Random.Range(mesh.bounds.min.z, mesh.bounds.max.z)
                );
                pointArray[i] = randomPos;
            }
            return pointArray;
        }
    }
}
