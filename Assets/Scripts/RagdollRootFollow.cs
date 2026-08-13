using UnityEngine;
using UnityEngine.UIElements;

public class RagdollRootFollow : MonoBehaviour
{
    public Transform physicalHips;
    public Transform rootBone;
    public Transform ragdollContainer;
    private Vector3 _offset;

    void Start()
    {
        if (physicalHips != null && rootBone != null)
        {
            // 1. Calculamos la distancia original
            _offset = rootBone.position - physicalHips.position;

            // 2. Metemos la cadera física dentro de tu contenedor organizado
            if (ragdollContainer != null)
            {
                physicalHips.SetParent(ragdollContainer);
            }
            else
            {
                // Si se te olvida asignar el contenedor, lo suelta en la escena para que no explote
                physicalHips.SetParent(null);
            }
        }
    }

    // Usamos LateUpdate para actualizar la malla DESPUÉS de las físicas
    void LateUpdate()
    {
        if (physicalHips != null && rootBone != null)
        {
            // 3. Movemos el Root para que viaje junto al ragdoll, evitando que se estire la piel
            rootBone.position = physicalHips.position + _offset;
        }
    }
}