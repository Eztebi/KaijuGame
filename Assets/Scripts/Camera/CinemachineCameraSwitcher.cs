using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineCameraSwitcher : MonoBehaviour
{
    private CinemachineCamera vCam;

    [Header("Prioridades")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 10;

    private void Awake()
    {
        vCam = GetComponent<CinemachineCamera>();
        DeactivateCamera();
    }

    public void ActivateCamera()
    {
        vCam.Priority = activePriority;
    }

    public void DeactivateCamera()
    {
        vCam.Priority = inactivePriority;
    }
}