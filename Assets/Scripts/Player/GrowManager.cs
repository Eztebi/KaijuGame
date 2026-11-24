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
    private Dictionary<ConfigurableJoint, Vector3> initialAnchors = new Dictionary<ConfigurableJoint, Vector3>();
    private Dictionary<ConfigurableJoint, Vector3> initialConnectedAnchors = new Dictionary<ConfigurableJoint, Vector3>();
    private Dictionary<ConfigurableJoint, float> initialLinearLimits = new Dictionary<ConfigurableJoint, float>();
    private Dictionary<Rigidbody, float> initialMasses = new Dictionary<Rigidbody, float>();

    public float CurrentSpeed => currentMoveSpeed;
    public float CurrentScale => currentScale;

    private InputHandler handler;

    private void Awake()
    {
        initialSprings.Clear();
        initialDampers.Clear();
        initialAnchors.Clear();
        initialConnectedAnchors.Clear();
        initialLinearLimits.Clear();
        initialMasses.Clear();

        // Solo lectura: NO modificamos ningún componente aquí para no romper el inicio en Play
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            initialMasses[rb] = rb.mass;
        }

        if (allJoints != null)
        {
            foreach (ConfigurableJoint joint in allJoints)
            {
                if (joint == null) continue;

                initialSprings[joint] = joint.slerpDrive.positionSpring;
                initialDampers[joint] = joint.slerpDrive.positionDamper;
                initialLinearLimits[joint] = joint.linearLimit.limit;
                initialAnchors[joint] = joint.anchor;

                if (joint.connectedBody != null)
                {
                    // Calculamos el punto exacto de conexión en espacio local del hueso padre
                    Vector3 worldAnchor = joint.transform.TransformPoint(joint.anchor);
                    initialConnectedAnchors[joint] = joint.connectedBody.transform.InverseTransformPoint(worldAnchor);
                }
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

    public void SetScale(float newScale)
    {
        currentScale = Mathf.Max(0.1f, newScale);

        // 1. Escala al padre (escala tanto el rig estático como el ragdoll)
        transform.localScale = Vector3.one * currentScale;

        // 2. Actualiza matrices y colliders antes de tocar PhysX
        Physics.SyncTransforms();

        // 3. Actualiza masa e inercia de todos los Rigidbodies
        foreach (var entry in initialMasses)
        {
            Rigidbody rb = entry.Key;
            if (rb == null) continue;

            rb.mass = entry.Value * currentScale;
            rb.ResetCenterOfMass();
            rb.ResetInertiaTensor();
        }

        // 4. Reconstruye los joints en PhysX con la nueva escala
        if (allJoints != null)
        {
            foreach (ConfigurableJoint joint in allJoints)
            {
                if (joint == null) continue;
                UpdateJointPhysics(joint);
            }
        }

        CallGrowStates();
    }

    private void UpdateJointPhysics(ConfigurableJoint joint)
    {
        if (joint.connectedBody != null)
        {
            joint.autoConfigureConnectedAnchor = false;

            if (initialAnchors.TryGetValue(joint, out Vector3 anchor))
                joint.anchor = anchor;

            if (initialConnectedAnchors.TryGetValue(joint, out Vector3 connectedAnchor))
                joint.connectedAnchor = connectedAnchor;

            // Obliga a PhysX a reconstruir el joint con el nuevo lossyScale
            Rigidbody connected = joint.connectedBody;
            joint.connectedBody = null;
            joint.connectedBody = connected;
        }

        if (initialLinearLimits.TryGetValue(joint, out float baseLimit))
        {
            SoftJointLimit limit = joint.linearLimit;
            limit.limit = baseLimit * currentScale;
            joint.linearLimit = limit;
        }

        JointDrive drive = new JointDrive
        {
            positionSpring = GetJointSpring(joint),
            positionDamper = GetJointDamper(joint),
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
        if (handler == null)
            handler = GetComponentInChildren<InputHandler>();

        if (handler != null)
            handler.BodyActiveState = InputHandler.EBodyActiveState.Grow;
    }
}