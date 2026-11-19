using System;
using UnityEngine;

public class GrowManager : MonoBehaviour
{
    [Header("Valores Actuales")]
    [SerializeField] private float currentScale = 1f;
    [SerializeField] private float currentSpring = 15000f;
    [SerializeField] private float currentDamper = 15000f;
    [SerializeField] private float currentMoveSpeed = 3f;

    [Header("Valores Base")]
    [SerializeField] private float baseSpring = 15000f;
    [SerializeField] private float baseDamper = 500f;
    [SerializeField] private float baseMoveSpeed = 3f;

    [Header("Referencias")]
    [SerializeField] private ConfigurableJoint[] allJoints;
    [SerializeField] private BodyStateMachineContext bodyContext;
    public float CurrentDamper => currentDamper;
    public float CurrentSpring => currentSpring;
    public float CurrentSpeed => 3f;
    public float CurrentScale => currentScale;
    public void SetScale(float newScale)
    {
        currentScale = Mathf.Max(0.1f, newScale);

        Transform parent = transform.root;
        parent.localScale = Vector3.one * currentScale;

        float scaledSpring = baseSpring * (currentScale * currentScale);
        float scaledDamper = baseDamper * currentScale;

        UpdateJointDrives(scaledSpring, scaledDamper);

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
    public void SetSpeed(float value)
    {
        currentMoveSpeed = value;
    }
}