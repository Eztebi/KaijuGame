using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

namespace EsteboFracture.Editor
{
    public class EsteboFractrureEditor : EditorWindow
    {
        [Header("Mesh Configuration")]
        [SerializeField] private GameObject mesh;
        [SerializeField] private EFractureType fractureType = EFractureType.Voronoi;

        [SerializeField] private float objectMass = 1f;
        [SerializeField] private float fragmentHealth = 100f;

        [SerializeField] private Material insideMaterial;

        [SerializeField] private FractureParameters voronoiParams;
        [SerializeField] private FractureParameters clusteredParams;

        private GameObject tempMesh;
        private GameObject fracturedResultMesh;

        private enum EGenerateType
        {
            Generate,
            CreatePrefab,
        }

        private FractureParameters CurrentSelectedParams
        {
            get
            {
                return fractureType switch
                {
                    EFractureType.Voronoi => voronoiParams,
                    EFractureType.Clustered => clusteredParams,
                    _ => throw new NotImplementedException($"Selected FractureType not implemented yet.")
                };
            }
        }

        private const string _prefabsFolderName = "DestructibleMeshes";

        [MenuItem("Tools/Estebo Fracture")]
        public static void OpenEditor()
        {
            GetWindow<EsteboFractrureEditor>("Estebo Fracture Editor");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Estebo Fracture", CustomEditorStyle.TitleStyle);
            GUILayout.Space(3);

            if (Application.isPlaying)
            {
                GUILayout.Label("ON PLAY MODE", GUI.skin.box, GUILayout.ExpandWidth(true));
                return;
            }

            EditorGUI.BeginChangeCheck();
            tempMesh = EditorGUILayout.ObjectField("Source", tempMesh, typeof(GameObject), true) as GameObject;

            if (EditorGUI.EndChangeCheck())
            {
                SetSource(tempMesh);
            }

            var clicked = GUILayout.Button("Set selected object as Source");
            var selected = Selection.activeGameObject;
            if (clicked && selected != null)
            {
                SetSource(selected);
            }

            GUILayout.Space(15);

            // 1. Mostrar parámetros generales
            GeneralGUIParams();

            // 2. Validación de mesh
            if (!mesh)
            {
                EditorGUILayout.HelpBox("Asigna un objeto GameObject en 'Source' para poder generar o crear prefabs.", MessageType.Info);
                return;
            }

            GUILayout.Space(10);

            bool canCreate = fractureType switch
            {
                EFractureType.Voronoi => VoronoiGUI(),
                EFractureType.Clustered => VoronoiGUI(),
                _ => false
            };

            if (canCreate)
            {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Generate", GUILayout.Height(30)))
                {
                    //MODO PRUEBA guarda y borra el mesh
                    if (fracturedResultMesh != null)
                    {
                        DestroyImmediate(fracturedResultMesh);
                    }

                    fracturedResultMesh = Instantiate(mesh);
                    fracturedResultMesh.name = $"{mesh.name}_Fractured_Preview";
                    mesh.SetActive(false); //Ocultamos el original mientras probamos
                    Debug.Log("Objeto de prueba generado en la escena.");
                }

                if (GUILayout.Button("Create Prefab", GUILayout.Height(30)))
                {
                    if (fracturedResultMesh != null)
                    {
                        SaveFracturedPrefab(mesh, fracturedResultMesh);
                        fracturedResultMesh = null; // Limpiamos la referencia
                    }
                    else
                    {
                        Debug.LogWarning("Debes presionar 'Generate' primero antes de crear el Prefab.");
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();

            Color prevBg = GUI.backgroundColor;
            Color prevContent = GUI.contentColor;

            GUI.backgroundColor = Color.red;
            GUI.contentColor = Color.white;

            if (GUILayout.Button("Deselect", GUILayout.Height(30)))
            {
                RemoveSelectedMesh();
            }

            GUI.backgroundColor = prevBg;
            GUI.contentColor = prevContent;
            
            GUILayout.EndHorizontal();
        }

        private void GeneralGUIParams()
        {
            EditorGUILayout.LabelField("Object Properties", CustomEditorStyle.HeaderStyleLeft);

            EditorGUI.BeginChangeCheck();

            objectMass = EditorGUILayout.FloatField("Object Mass", objectMass);
            fragmentHealth = EditorGUILayout.FloatField("Fragments Health", fragmentHealth);

            if (EditorGUI.EndChangeCheck())
            {
                //AssignParams();
            }

            EditorGUI.BeginChangeCheck();

            insideMaterial = (Material)EditorGUILayout.ObjectField("Inside Material", insideMaterial, typeof(Material), false);
            if (!insideMaterial)
            {
                EditorGUILayout.HelpBox("If material is not assigned, the same material from the object will be assigned", MessageType.Info);
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Fracture Properties", CustomEditorStyle.HeaderStyleLeft);
        }

        private bool VoronoiGUI()
        {
            EditorGUILayout.LabelField("Voronoi Fracture", CustomEditorStyle.HeaderStyleCentered);

            if (GUILayout.Button("Visualize Points"))
            {

                // Visualización de puntos
            }
            return true;
        }

        private void SetSource(GameObject source)
        {
            mesh = source;
            tempMesh = source;
        }
        private void RemoveSelectedMesh()
        {
            mesh = null;
            tempMesh = null;
            fracturedResultMesh = null;
        }
        private void SaveFracturedPrefab(GameObject originalObject, GameObject fracturedObject)
        {
            if (originalObject == null || fracturedObject == null) return;

            // Preparar carpetas
            if (!AssetDatabase.IsValidFolder($"Assets/{_prefabsFolderName}")) AssetDatabase.CreateFolder("Assets", $"{_prefabsFolderName}");
            if (!AssetDatabase.IsValidFolder($"Assets/{_prefabsFolderName}/Meshes")) AssetDatabase.CreateFolder($"Assets/{_prefabsFolderName}", "Meshes");
            if (!AssetDatabase.IsValidFolder($"Assets/{_prefabsFolderName}/Prefabs")) AssetDatabase.CreateFolder($"Assets/{_prefabsFolderName}", "Prefabs");

            FileUtil.DeleteFileOrDirectory($"Assets/{_prefabsFolderName}/Meshes/{originalObject.name}");
            FileUtil.DeleteFileOrDirectory($"Assets/{_prefabsFolderName}/Meshes/{originalObject.name}.meta");
            AssetDatabase.Refresh();
            AssetDatabase.CreateFolder($"Assets/{_prefabsFolderName}/Meshes", originalObject.name);

            fracturedObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            // Guardar mallas (Soporta si el objeto tiene hijos o si es solo un MeshFilter raíz para la prueba)
            int i = 0;

            // Si el objeto actual tiene MeshFilter (para pruebas con un objeto simple)
            var rootMeshFilter = fracturedObject.GetComponent<MeshFilter>();
            if (rootMeshFilter != null && rootMeshFilter.sharedMesh != null)
            {
                Mesh meshCopy = Instantiate(rootMeshFilter.sharedMesh);
                AssetDatabase.CreateAsset(meshCopy, $"Assets/{_prefabsFolderName}/Meshes/{originalObject.name}/{originalObject.name}_MainMesh.asset");
                rootMeshFilter.sharedMesh = meshCopy;
            }

            // Si tiene hijos (chunks)
            foreach (Transform chunk in fracturedObject.transform)
            {
                var meshFilter = chunk.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    Mesh meshCopy = Instantiate(meshFilter.sharedMesh);
                    AssetDatabase.CreateAsset(meshCopy, $"Assets/{_prefabsFolderName}/Meshes/{originalObject.name}/{originalObject.name}_Chunk{i}.asset");
                    meshFilter.sharedMesh = meshCopy;
                }
                i++;
            }

            AssetDatabase.Refresh();

            // Guardar el Prefab fracturado
            string prefabPath = $"Assets/{_prefabsFolderName}/Prefabs/{originalObject.name}_Fractured.prefab";
            GameObject fracturedPrefab = PrefabUtility.SaveAsPrefabAsset(fracturedObject, prefabPath);

            // Instanciar el prefab guardado en la escena
            GameObject instantiated = PrefabUtility.InstantiatePrefab(fracturedPrefab) as GameObject;
            if (instantiated != null)
            {
                instantiated.transform.SetPositionAndRotation(originalObject.transform.position, originalObject.transform.rotation);
            }

            // Solo destruimos el objeto temporal si NO es el original
            if (fracturedObject != originalObject)
            {
                DestroyImmediate(fracturedObject);
            }

            // Reactivamos el objeto original en la escena
            if (originalObject != null)
            {
                originalObject.SetActive(true);
            }

            RemoveSelectedMesh();
            Debug.Log($"Prefab y mallas guardados con éxito en Assets/{_prefabsFolderName}");
        }
    }
}