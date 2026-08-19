using Unity.VisualScripting;
using UnityEngine;

public class RigidbodiesSolvers : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rbs;
    [SerializeField] private int solverIterations;
    [SerializeField] private int solverVelocity;
    [SerializeField] private float maxAngularVelocity;

    private void Start()
    {
        SetSolvers();
    }
    void SetSolvers()
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.solverIterations = solverIterations;
            rb.solverVelocityIterations = solverVelocity;
            rb.maxAngularVelocity = maxAngularVelocity;
        }
    }
}
