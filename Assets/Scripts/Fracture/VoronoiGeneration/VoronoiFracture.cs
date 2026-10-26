using UnityEngine;

namespace EsteboFracture
{
    public class VoronoiFracture : MonoBehaviour
    { 
        Mesh mesh;
        FractureParameters parameters;

        public VoronoiFracture(Mesh mesh, FractureParameters parameters)
        {
            this.mesh = mesh;
            this.parameters = parameters;
        }

        public void GenerateFracture()
        {
            PointGenerator.GeneratePoints(parameters.generationType, parameters.cells, parameters.points, mesh);
        }
        void OnDrawGizmos()
        {

            for (int i = 0; i < parameters.cells.Length; i++)
            {
                Gizmos.color = UnityEngine.Color.yellow;
                Gizmos.DrawSphere(parameters.cells[i].GetSite.Position, 1);
            }
        }
    }
}