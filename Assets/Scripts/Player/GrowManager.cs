using System;
using System.Collections.Generic;
using UnityEngine;

public class GrowManager : MonoBehaviour
{
    [Header("Valores Actuales")]
    [SerializeField] private float currentScale = 1f;
    [SerializeField] private float currentSpring = 15000f;
    [SerializeField] private float currentDamper = 500f;
    [SerializeField] private float currentMoveSpeed = 3f;

    [Header("Referencias")]
    [SerializeField] private ConfigurableJoint[] allJoints;

    private Dictionary<ConfigurableJoint, float> initialSprings = new Dictionary<ConfigurableJoint, float>();
    private Dictionary<ConfigurableJoint, float> initialDampers = new Dictionary<ConfigurableJoint, float>();
    public float CurrentSpeed => 3f;
    public float CurrentScale => currentScale;

    private InputHandler handler;
    private void Awake()
    {
        initialSprings.Clear();
        initialDampers.Clear();

        if (allJoints == null) return;

        foreach (ConfigurableJoint joint in allJoints)
        {
            if (joint == null) continue;

            initialSprings[joint] = joint.slerpDrive.positionSpring;
            initialDampers[joint] = joint.slerpDrive.positionDamper;
        }

        handler = GetComponentInChildren<InputHandler>();

    }
    public float GetJointSpring(ConfigurableJoint joint)
    {
        if (joint != null && initialSprings.TryGetValue(joint, out float baseValue))
        {
            return baseValue * currentScale;
        }

        return currentSpring;
    }

    public float GetJointDamper(ConfigurableJoint joint)
    {
        if (joint != null && initialDampers.TryGetValue(joint, out float baseValue))
        {
            return baseValue * currentScale;
        }

        return currentDamper;
    }
    public void SetScale(float newScale)
    {
        currentScale = Mathf.Max(0.1f, newScale);

        gameObject.transform.localScale = Vector3.one * currentScale;

        
        foreach(ConfigurableJoint joint in allJoints)
        {
            UpdateJointDrive(joint);
        }
        CallGrowStates();
    }

    private void UpdateJointDrive(ConfigurableJoint joint)
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = GetJointSpring(joint) * currentScale,
            positionDamper = GetJointDamper(joint) * currentScale,
            maximumForce = float.MaxValue
        };
        joint.slerpDrive = drive;
    }
    public void SetSpeed(float value)
    {
        currentMoveSpeed = value;
    }
    public void CallGrowStates()
    {
        if(handler == null)
        {
            handler = GetComponentInChildren<InputHandler>();
        }

        handler.BodyActiveState = InputHandler.EBodyActiveState.Grow;
    }
}