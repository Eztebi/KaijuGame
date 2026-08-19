using UnityEngine;

public class FixMeshStretching : MonoBehaviour
{
    [Header("Referencias")]
    public SkinnedMeshRenderer meshRenderer;
    public Transform rootBone; // Arrastra el hueso 'Root'
    public Transform hipsBone; // Arrastra el hueso 'Hips'

    void Awake()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (meshRenderer == null || rootBone == null || hipsBone == null) return;

        // 1. Clonamos la malla en memoria para no modificar el archivo FBX original en disco
        Mesh newMesh = Instantiate(meshRenderer.sharedMesh);
        Transform[] bones = meshRenderer.bones;

        int rootIndex = -1;
        int hipsIndex = -1;

        // 2. Buscamos los índices de ambos huesos en la malla
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] == rootBone) rootIndex = i;
            if (bones[i] == hipsBone) hipsIndex = i;
        }

        if (rootIndex == -1 || hipsIndex == -1)
        {
            Debug.LogWarning("FixMeshStretching: No se encontraron los huesos en el SkinnedMeshRenderer.");
            return;
        }

        // 3. Reasignamos los pesos: cualquier vértice ligado a 'Root' ahora se liga a 'Hips'
        BoneWeight[] weights = newMesh.boneWeights;

        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i].boneIndex0 == rootIndex) weights[i].boneIndex0 = hipsIndex;
            if (weights[i].boneIndex1 == rootIndex) weights[i].boneIndex1 = hipsIndex;
            if (weights[i].boneIndex2 == rootIndex) weights[i].boneIndex2 = hipsIndex;
            if (weights[i].boneIndex3 == rootIndex) weights[i].boneIndex3 = hipsIndex;
        }

        newMesh.boneWeights = weights;
        meshRenderer.sharedMesh = newMesh;

        Debug.Log("<color=green>¡Éxito!</color> Vértices del hueso Root reasignados a Hips. Estiramiento eliminado.");
    }
}