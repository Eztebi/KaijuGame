using UnityEngine;

namespace EsteboFracture
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class VoronoiFracture : MonoBehaviour
    {
        [SerializeField]private FractureParameters parameters = new FractureParameters();
        public FractureParameters Parameters
        {
            get => parameters;
            set => parameters = value;
        }
        public Mesh GetMesh()
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            return filter != null ? filter.sharedMesh : null;
        }

        public void GenerateFracture()
        {
            Mesh mesh = GetMesh();
            if (mesh == null)
            {
                Debug.LogError("No se encontró un Mesh en el MeshFilter.");
                return;
            }

            parameters.cells = PointGenerator.GeneratePoints(parameters.generationType, parameters.points, mesh);
        }
        private void OnDrawGizmos()
        {
            if (parameters == null || !parameters.showGizmos || parameters.cells == null) return;

            Gizmos.color = parameters.gizmoColor;
            for (int i = 0; i < parameters.cells.Length; i++)
            {
                if (parameters.cells[i] != null && parameters.cells[i].GetSite != null)
                {
                    Vector3 worldPos = transform.TransformPoint(parameters.cells[i].GetSite.Position);
                    Gizmos.DrawSphere(worldPos, parameters.gizmoRadius);
                }
            }
        }
    }
}