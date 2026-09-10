using UnityEngine;

public class GrowManager : MonoBehaviour
{
    [Header("Escala Actual")]
    [SerializeField] private float currentScale = 1f;

    [Header("Valores Base")]
    [SerializeField] private float baseSpring = 15000f;
    [SerializeField] private float baseDamper = 500f;
    [SerializeField] private float baseMoveSpeed = 3f;

    [Header("Referencias")]
    [SerializeField] private ConfigurableJoint[] allJoints;
    [SerializeField] private BodyStateMachineContext bodyContext;

    public float CurrentScale => currentScale;

    public void SetScale(float newScale)
    {
        currentScale = Mathf.Max(0.1f, newScale);

        transform.localScale = Vector3.one * currentScale;

        float scaledSpring = baseSpring * (currentScale * currentScale);
        float scaledDamper = baseDamper * currentScale;

        UpdateJointDrives(scaledSpring, scaledDamper);

        if (bodyContext != null)
        {
            //bodyContext.scaledMoveSpeed = baseMoveSpeed * currentScale;
        }
    }

    private void UpdateJointDrives(float spring, float damper)
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = spring,
            positionDamper = damper,
            maximumForce = float.MaxValue
        };

        foreach (var joint in allJoints)
        {
            if (joint != null)
            {
                joint.slerpDrive = drive;
            }
        }
    }
}