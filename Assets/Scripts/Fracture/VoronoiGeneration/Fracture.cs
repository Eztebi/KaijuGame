using System.Collections.Generic;
using UnityEngine;

namespace EsteboFracture
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Fracture : MonoBehaviour
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
            List<Mesh> fragments = new List<Mesh>();

            switch (parameters.fractureType)
            {
                case EFractureType.Voronoi:
                    EsteboFracture.FractureGenerator.VoronoiFracture();
                    break;
                case EFractureType.Slicer:
                    fragments = EsteboFracture.FractureGenerator.SliceFracture(mesh, parameters.cells);
                    break;
                default:
                    break;
            }
            if (fragments != null && fragments.Count > 0)
            {
                MeshRenderer originalRenderer = GetComponent<MeshRenderer>();
                if (originalRenderer != null) originalRenderer.enabled = false;

                for (int i = 0; i < fragments.Count; i++)
                {
                    GameObject fragmentObj = new GameObject($"Fragment_{i}");

                    fragmentObj.transform.SetParent(this.transform);
                    fragmentObj.transform.localPosition = Vector3.zero;
                    fragmentObj.transform.localRotation = Quaternion.identity;
                    fragmentObj.transform.localScale = Vector3.one;

                    MeshFilter mf = fragmentObj.AddComponent<MeshFilter>();
                    mf.sharedMesh = fragments[i];

                    MeshRenderer mr = fragmentObj.AddComponent<MeshRenderer>();
                    if (originalRenderer != null)
                    {
                        mr.sharedMaterials = originalRenderer.sharedMaterials;
                    }
                    fragmentObj.AddComponent<MeshCollider>().convex = true;
                    Rigidbody rb = fragmentObj.AddComponent<Rigidbody>();
                    rb.useGravity = false;    
                    rb.isKinematic = true;
                    rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    fragmentObj.AddComponent<Fragment>();
                    fragmentObj.tag = "Fragment";
                }
                MeshSlicer.ConnectNeighbors(new List<Fragment>(GetComponentsInChildren<Fragment>()));
            }
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