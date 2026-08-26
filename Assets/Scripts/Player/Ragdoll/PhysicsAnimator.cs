using System.Collections.Generic;
using UnityEngine;

public class PhysicsAnimator : MonoBehaviour
{

    private Transform[] _bones;
    private Dictionary<Transform, ConfigurableJoint> _configurableJoints = new();
    private Dictionary<Transform, Quaternion> _initialRotations = new();
    private Dictionary<Transform, Quaternion> _preAnimationRotations = new();
    private Dictionary<Transform, Vector3> _preAnimationPositions = new();
    private Dictionary<Transform, Quaternion> _animatedRotations = new();

    void Start()
    {
        _bones = transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < _bones.Length; i++)
        {
            if (_bones[i].TryGetComponent(out ConfigurableJoint joint))
            {
                _configurableJoints[_bones[i]] = joint;
                _initialRotations[_bones[i]] = _bones[i].localRotation;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < _bones.Length; i++)
        {

            _preAnimationRotations[_bones[i]] = _bones[i].localRotation;
            _preAnimationPositions[_bones[i]] = _bones[i].position;
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            _animatedRotations[_bones[i]] = _bones[i].localRotation;
        }

        for (int i = 0; i < _bones.Length; i++)
        {

            _bones[i].localRotation = _preAnimationRotations[_bones[i]];
            _bones[i].position = _preAnimationPositions[_bones[i]];
        }

        foreach (Transform bone in _configurableJoints.Keys)
        {
            _configurableJoints[bone].SetTargetRotationLocal(_animatedRotations[bone], _initialRotations[bone]);
        }
    }
}