using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class StaticBoneFollower : MonoBehaviour
{
    public Transform staticBone;

    private ConfigurableJoint _joint;
    private Quaternion _startLocalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _joint = GetComponent<ConfigurableJoint>();
        _startLocalRotation = transform.localRotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(staticBone != null && _joint != null)
        {
            _joint.SetTargetRotationLocal(staticBone.localRotation, _startLocalRotation);
        }
    }
}
