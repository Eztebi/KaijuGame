using System.Collections.Generic;
using UnityEngine;

namespace EsteboFracture
{
    public static class MeshSlicer
    {
        public static Mesh Slice(Mesh currentFragment, Plane plane)
        {
            //Get data of the mesh
            Vector3[] vertices = currentFragment.GetVertices();
            Vector3[] normals = currentFragment.GetNormals();
            Vector2[] uvs = currentFragment.GetUVs();
            int[] triangles = currentFragment.GetTriangles();

            //Data of new mesh
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector3> newNormals = new List<Vector3>();
            List<Vector2> newUVs = new List<Vector2>();
            List<int> newTriangles = new List<int>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                //Get verticecs of triangle
                Vector3 vertexA = vertices[triangles[i]];
                Vector3 vertexB = vertices[triangles[i + 1]];
                Vector3 vertexC = vertices[triangles[i + 2]];
                //Normals of triangle
                Vector3 normalA = normals[triangles[i]];
                Vector3 normalB = normals[triangles[i + 1]];
                Vector3 normalC = normals[triangles[i + 2]];
                //UVs of triangle
                Vector2 uvA = uvs[triangles[i]];
                Vector2 uvB = uvs[triangles[i + 1]];
                Vector2 uvC = uvs[triangles[i + 2]];

                float distanceA = plane.GetDistanceToPoint(vertexA);
                float distanceB = plane.GetDistanceToPoint(vertexB);
                float distanceC = plane.GetDistanceToPoint(vertexC);

                bool isAInside = distanceA < 0;
                bool isBInside = distanceB < 0;
                bool isCInside = distanceC < 0;

                int insideCount = (isAInside ? 1 : 0) + (isBInside ? 1 : 0) + (isCInside ? 1 : 0);

                if (insideCount == 0)
                {
                    continue;
                }
                else if (insideCount == 3)
                {
                    AddTriangle(newVertices, newNormals, newUVs, newTriangles, vertexA, vertexB, vertexC, normalA, normalB, normalC, uvA, uvB, uvC);
                }
                else if (insideCount == 2)
                {
                    ProcessTwoVertices(newVertices, newNormals, newUVs, newTriangles, vertexA, vertexB, vertexC, normalA, normalB, normalC, uvA, uvB, uvC, isAInside, isBInside, isCInside, distanceA, distanceB, distanceC);
                }
                else if (insideCount == 1)
                {
                    ProcessOneVertex(newVertices, newNormals, newUVs, newTriangles, vertexA, vertexB, vertexC, normalA, normalB, normalC, uvA, uvB, uvC, isAInside, isBInside, isCInside, distanceA, distanceB, distanceC);
                }
            }
            Mesh resultMesh = new Mesh();
            resultMesh.vertices = newVertices.ToArray();
            resultMesh.triangles = newTriangles.ToArray();
            resultMesh.uv = newUVs.ToArray();
            resultMesh.normals = newNormals.ToArray();

            resultMesh.RecalculateBounds();
            return resultMesh;
        }
        private static void AddTriangle(List<Vector3> newVertices, List<Vector3> newNormals, List<Vector2> newUVs, List<int> newTriangles,
            Vector3 vA, Vector3 vB, Vector3 vC,
            Vector3 nA, Vector3 nB, Vector3 nC,
            Vector2 uvA, Vector2 uvB, Vector2 uvC)
        {
            int startIndex = newVertices.Count;

            newVertices.Add(vA);
            newVertices.Add(vB); 
            newVertices.Add(vC);
            newNormals.Add(nA); 
            newNormals.Add(nB); 
            newNormals.Add(nC);
            newUVs.Add(uvA); 
            newUVs.Add(uvB); 
            newUVs.Add(uvC);

            newTriangles.Add(startIndex);
            newTriangles.Add(startIndex + 1);
            newTriangles.Add(startIndex + 2);
        }

        private static void ProcessOneVertex(List<Vector3> newVertices, List<Vector3> newNormals, List<Vector2> newUVs, List<int> newTriangles,
            Vector3 vA, Vector3 vB, Vector3 vC, Vector3 nA, Vector3 nB, Vector3 nC, Vector2 uvA, Vector2 uvB, Vector2 uvC,
            bool isAInside, bool isBInside, bool isCInside, float distA, float distB, float distC)
        {
            if (isAInside)
            {
                float t1 = Mathf.Abs(distA) / (Mathf.Abs(distA) + Mathf.Abs(distB));
                float t2 = Mathf.Abs(distA) / (Mathf.Abs(distA) + Mathf.Abs(distC));

                AddTriangle(newVertices, newNormals, newUVs, newTriangles,
                    vA, Vector3.Lerp(vA, vB, t1), Vector3.Lerp(vA, vC, t2),
                    nA, Vector3.Lerp(nA, nB, t1).normalized, Vector3.Lerp(nA, nC, t2).normalized,
                    uvA, Vector2.Lerp(uvA, uvB, t1), Vector2.Lerp(uvA, uvC, t2));
            }
            else if (isBInside)
            {
                float t1 = Mathf.Abs(distB) / (Mathf.Abs(distB) + Mathf.Abs(distC));
                float t2 = Mathf.Abs(distB) / (Mathf.Abs(distB) + Mathf.Abs(distA));

                AddTriangle(newVertices, newNormals, newUVs, newTriangles,
                    vB, Vector3.Lerp(vB, vC, t1), Vector3.Lerp(vB, vA, t2),
                    nB, Vector3.Lerp(nB, nC, t1).normalized, Vector3.Lerp(nB, nA, t2).normalized,
                    uvB, Vector2.Lerp(uvB, uvC, t1), Vector2.Lerp(uvB, uvA, t2));
            }
            else if (isCInside)
            {
                float t1 = Mathf.Abs(distC) / (Mathf.Abs(distC) + Mathf.Abs(distA));
                float t2 = Mathf.Abs(distC) / (Mathf.Abs(distC) + Mathf.Abs(distB));

                AddTriangle(newVertices, newNormals, newUVs, newTriangles,
                    vC, Vector3.Lerp(vC, vA, t1), Vector3.Lerp(vC, vB, t2),
                    nC, Vector3.Lerp(nC, nA, t1).normalized, Vector3.Lerp(nC, nB, t2).normalized,
                    uvC, Vector2.Lerp(uvC, uvA, t1), Vector2.Lerp(uvC, uvB, t2));
            }
        }

        private static void ProcessTwoVertices(List<Vector3> newVertices, List<Vector3> newNormals, List<Vector2> newUVs, List<int> newTriangles,
            Vector3 vA, Vector3 vB, Vector3 vC, Vector3 nA, Vector3 nB, Vector3 nC, Vector2 uvA, Vector2 uvB, Vector2 uvC,
            bool isAInside, bool isBInside, bool isCInside, float distA, float distB, float distC)
        {
            if (!isCInside)
            {
                float t1 = Mathf.Abs(distA) / (Mathf.Abs(distA) + Mathf.Abs(distC));
                float t2 = Mathf.Abs(distB) / (Mathf.Abs(distB) + Mathf.Abs(distC));

                Vector3 cutAC = Vector3.Lerp(vA, vC, t1);
                Vector3 cutBC = Vector3.Lerp(vB, vC, t2);

                Vector3 normAC = Vector3.Lerp(nA, nC, t1).normalized;
                Vector3 normBC = Vector3.Lerp(nB, nC, t2).normalized;

                Vector2 uvAC = Vector2.Lerp(uvA, uvC, t1);
                Vector2 uvBC = Vector2.Lerp(uvB, uvC, t2);

                AddTriangle(newVertices, newNormals, newUVs, newTriangles, vA, vB, cutAC, nA, nB, normAC, uvA, uvB, uvAC);
                AddTriangle(newVertices, newNormals, newUVs, newTriangles, vB, cutBC, cutAC, nB, normBC, normAC, uvB, uvBC, uvAC);
            }
            else if (!isAInside)
            {
                float t1 = Mathf.Abs(distB) / (Mathf.Abs(distB) + Mathf.Abs(distA));
                float t2 = Mathf.Abs(distC) / (Mathf.Abs(distC) + Mathf.Abs(distA));

                Vector3 cutBA = Vector3.Lerp(vB, vA, t1);
                Vector3 cutCA = Vector3.Lerp(vC, vA, t2);

                Vector3 normBA = Vector3.Lerp(nB, nA, t1).normalized;
                Vector3 normCA = Vector3.Lerp(nC, nA, t2).normalized;

                Vector2 uvBA = Vector2.Lerp(uvB, uvA, t1);
                Vector2 uvCA = Vector2.Lerp(uvC, uvA, t2);
    
                AddTriangle(newVertices, newNormals, newUVs, newTriangles, vB, vC, cutBA, nB, nC, normBA, uvB, uvC, uvBA);
                AddTriangle(newVertices, newNormals, newUVs, newTriangles, vC, cutCA, cutBA, nC, normCA, normBA, uvC, uvCA, uvBA);
            }
            else if (!isBInside)
            {
                float t1 = Mathf.Abs(distC) / (Mathf.Abs(distC) + Mathf.Abs(distB));
                float t2 = Mathf.Abs(distA) / (Mathf.Abs(distA) + Mathf.Abs(distB));

                Vector3 cutCB = Vector3.Lerp(vC, vB, t1);
                Vector3 cutAB = Vector3.Lerp(vA, vB, t2);

                Vector3 normCB = Vector3.Lerp(nC, nB, t1).normalized;
                Vector3 normAB = Vector3.Lerp(nA, nB, t2).normalized;

                Vector2 uvCB = Vector2.Lerp(uvC, uvB, t1);
                Vector2 uvAB = Vector2.Lerp(uvA, uvB, t2);

                AddTriangle(newVertices, newNormals, newUVs, newTriangles, vC, vA, cutCB, nC, nA, normCB, uvC, uvA, uvCB);
                AddTriangle(newVertices, newNormals, newUVs, newTriangles, vA, cutAB, cutCB, nA, normAB, normCB, uvA, uvAB, uvCB);
            }
        }
        public static void ConnectNeighbors(List<Fragment> allFragments)
        {
            for (int i = 0; i < allFragments.Count; i++)
            {
                Fragment fragA = allFragments[i];
                Renderer renderA = fragA.GetComponent<Renderer>();

                for (int j = i + 1; j < allFragments.Count; j++)
                {
                    Fragment fragB = allFragments[j];

                    Renderer renderB = fragB.GetComponent<Renderer>();

                    if (!renderA.bounds.Intersects(renderB.bounds)) continue;

                    if (ShareVertices(fragA.GetComponent<MeshFilter>(), fragB.GetComponent<MeshFilter>(), fragA.transform, fragB.transform))
                    {
                        fragA.neighbors.Add(fragB);
                        fragB.neighbors.Add(fragA);
                    }
                }
            }
        }
        private static bool ShareVertices(MeshFilter filterA, MeshFilter filterB, Transform transformA, Transform transformB)
        {
            Vector3[] verticesA = filterA.sharedMesh.vertices;
            Vector3[] verticesB = filterB.sharedMesh.vertices;

            float threshold = 0.01f;

            foreach (Vector3 localVA in verticesA)
            {
                Vector3 worldVA = transformA.TransformPoint(localVA);

                foreach (Vector3 localVB in verticesB)
                {
                    Vector3 worldVB = transformB.TransformPoint(localVB);
                    if (Vector3.Distance(worldVA, worldVB) < threshold)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}


