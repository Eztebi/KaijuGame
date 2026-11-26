using System;
using System.Collections.Generic;
using UnityEngine;

public class GrowManager : MonoBehaviour
{
    [Header("Valores Actuales")]
    [SerializeField] private int currentScale = 1;
    [SerializeField] private float currentSpring = 15000f;
    [SerializeField] private float currentDamper = 500f;
    [SerializeField] private float currentMoveSpeed = 3f;

    [Header("Prefabs del Kaiju")]
    [SerializeField] private GameObject prefab1;
    [SerializeField] private GameObject prefab2;
    [SerializeField] private GameObject prefab3;
    [SerializeField] private GameObject prefab4;
    [SerializeField] private GameObject prefab5;
    [SerializeField] private GameObject prefab6;
    [SerializeField] private GameObject prefab7;
    [SerializeField] private ConfigurableJoint[] allJoints;

    private Dictionary<ConfigurableJoint, float> initialSprings = new Dictionary<ConfigurableJoint, float>();
    private Dictionary<ConfigurableJoint, float> initialDampers = new Dictionary<ConfigurableJoint, float>();
    private Dictionary<ConfigurableJoint, float> initialLinearLimits = new Dictionary<ConfigurableJoint, float>();

    public float CurrentSpeed => currentMoveSpeed;
    public int CurrentScale => currentScale;

    private InputHandler handler;

    private void Awake()
    {
        initialSprings.Clear();
        initialDampers.Clear();
        initialLinearLimits.Clear();

        if (allJoints != null)
        {
            foreach (ConfigurableJoint joint in allJoints)
            {
                if (joint == null) continue;

                initialSprings[joint] = joint.slerpDrive.positionSpring;
                initialDampers[joint] = joint.slerpDrive.positionDamper;
                initialLinearLimits[joint] = joint.linearLimit.limit;
            }
        }

        handler = GetComponentInChildren<InputHandler>();
    }

    public float GetJointSpring(ConfigurableJoint joint)
    {
        float scaleFactor = Mathf.Pow(currentScale, 2f);

        if (joint != null && initialSprings.TryGetValue(joint, out float baseValue))
            return baseValue * scaleFactor;

        return currentSpring * scaleFactor;
    }

    public float GetJointDamper(ConfigurableJoint joint)
    {
        float scaleFactor = Mathf.Pow(currentScale, 2f);

        if (joint != null && initialDampers.TryGetValue(joint, out float baseValue))
            return baseValue * scaleFactor;

        return currentDamper * scaleFactor;
    }
    public void SetScale(int value)
    {
        currentScale = value;

        this.gameObject.SetActive(false);
        GameObject newKaiju = Instantiate(prefab2, prefab1.transform.position, transform.rotation);
        Rigidbody[] rigidbodies = newKaiju.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }

        Destroy(prefab1);
        CallGrowStates();
    }
    public void CallGrowStates()
    {
        if (handler == null)
            handler = GetComponentInChildren<InputHandler>();

        if (handler != null)
            handler.BodyActiveState = InputHandler.EBodyActiveState.Grow;
    }
}