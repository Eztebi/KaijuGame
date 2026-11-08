using UnityEngine;

namespace EsteboFracture {
    public static class GeometryExtensions
    {
        public static Vector3[] GetVertices(this Mesh mesh)
        {
            return mesh.vertices;
        }
        public static int[] GetTriangles(this Mesh mesh)
        {
            return mesh.triangles;
        }
        public static Vector3[] GetNormals(this Mesh mesh)
        {
            return mesh.normals;
        }
        public static Vector2[] GetUVs(this Mesh mesh)
        {
            return mesh.uv;
        }
        public static void CreateTriangles(Vector3 point1, Vector3 point2, Vector3 point3, Mesh mesh)
        {
            //mesh.SetTriangles()
        }
    }
}
