using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyStateMachineContext 
{
    private InputHandler _handler;
    private Rigidbody[] _rigidbodies;
    private Animator _animator;
    private Transform _bodyPos;
    private StaticBoneFollower[] _bones;
    private CinemachineCamera _camera;
    public Rigidbody HipsRB { get; private set; }
    public Rigidbody SpineRB { get; private set; }
    public Rigidbody HeadRB { get; private set; }
    public ConfigurableJoint HipsJoint { get; private set; } // El que NO tiene Connected Body
    public ConfigurableJoint SpineJoint { get; private set; }
    public ConfigurableJoint HeadJoint { get; private set; }
    public ConfigurableJoint[] _armsJoints;
    private CinemachineOrbitalFollow _orbitalCamera;
    public BodyStateMachineContext(Transform bodypos, StaticBoneFollower[] bones,InputHandler handler, Rigidbody[] rbs, Animator animator,
        Rigidbody hipsRB, Rigidbody spineRB, Rigidbody headRB,
        ConfigurableJoint hipsJoint, ConfigurableJoint spineJoint, ConfigurableJoint headJoint, CinemachineCamera camera, CinemachineOrbitalFollow orbitalCamera)
    {
        _handler = handler;
        _rigidbodies = rbs;
        _animator = animator;
        _bodyPos = bodypos;
        _bones = bones;
        HipsRB = hipsRB;
        SpineRB = spineRB;
        HeadRB = headRB;
        HipsJoint = hipsJoint;
        SpineJoint = spineJoint;
        HeadJoint = headJoint;
        _camera = camera;
        _orbitalCamera = orbitalCamera;
    }
    public void SetBodyToIdle()
    {
        SetHipsBalance(15000f, 500f);

        SetJointSlerp(SpineJoint, 4000f, 150f);
        SetJointSlerp(HeadJoint, 1000f, 40f);
    }

    public void SetBodyToRagdoll()
    {
        SetHipsBalance(0f, 0f);

        SetJointSlerp(SpineJoint, 0f, 0f);
        SetJointSlerp(HeadJoint, 0f, 0f);
    }
    private void SetJointSlerp(ConfigurableJoint joint, float spring, float damper)
    {
        if (joint == null) return;
        JointDrive drive = joint.slerpDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        joint.slerpDrive = drive;
    }

    private void SetHipsBalance(float spring, float damper)
    {
        if (HipsJoint == null) return;

        JointDrive drive = HipsJoint.slerpDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        HipsJoint.slerpDrive = drive;
    }
    private void SetArmsBalance(float spring, float damper)
    {
        foreach(ConfigurableJoint muscle in _armsJoints)
        {
            JointDrive drive = muscle.slerpDrive;
            drive.positionSpring = spring;
            drive.positionDamper = damper;
            muscle.slerpDrive = drive;
        }
    }
public void SetBodyToReset()
    {

        SetHipsBalance(20000f, 1000f); 
        SetJointSlerp(SpineJoint, 10000f, 600f);
        SetJointSlerp(HeadJoint, 800f, 60f);
    }
    public void SetBoneStiffness(float spring, float damper)
    {
        if (_bones == null) return;

        foreach (var muscle in _bones)
        {
            muscle.SetMuscleStrength(spring, damper);
        }
    }
    public Transform FullBodyPos => _bodyPos;
    public InputHandler Handler => _handler;
    public CinemachineOrbitalFollow OrbitalCamera => _orbitalCamera;
}
