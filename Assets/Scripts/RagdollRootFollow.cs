using UnityEngine;
using UnityEngine.UIElements;

public class RagdollRootFollow : MonoBehaviour
{
    public Transform physicalModelRoot;

    [Tooltip("El hueso de la cadera (Hips) del modelo físico")]
    public Transform physicalHips;

    private Vector3 _offset;

    void Start()
    {
        // 1. Desvinculamos el modelo físico del Padre para evitar el bucle de físicas
        if (physicalModelRoot != null)
        {
            physicalModelRoot.SetParent(null);
        }

        // 2. Calculamos la distancia inicial
        if (physicalHips != null)
        {
            _offset = transform.position - physicalHips.position;
        }
    }

    // Usamos FixedUpdate porque estamos siguiendo objetos físicos (Rigidbodies)
    void FixedUpdate()
    {
        if (physicalHips != null)
        {
            // Movemos el contenedor (que ahora solo tiene al modelo Fantasma/IK)
            transform.position = physicalHips.position + _offset;
        }
    }
}