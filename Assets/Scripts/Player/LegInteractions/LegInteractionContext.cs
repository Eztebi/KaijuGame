using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static LegStateMachine;

public class LegInteractionContext
{
    private InputHandler _handler;
    private TwoBoneIKConstraint _ikConstraint;
    private MultiRotationConstraint _multiRotationConstraint;
    //private Rigidbody _rigidbody;
    //private CapsuleCollider _rootCollider;
    private BoxCollider _legCollider;

    private Transform _fullBodyPos;
    private Transform _legRestPosition;
    private Transform _legUpPosition;
    private Transform _feetRestPosition;
    private Transform _hipPosition;
    private Transform _hipNormalPos;
    private Transform _otherLegPosition;

    private Transform _legIKTargetTransform;
    private Transform _feetIKTargetTransform;
    private LegStateMachine.ELegType _legType;
    private bool _isGrounded;
    private InputHandler.EControllerInput _currentLegInput;
    public Vector3 _currentLegPos;
    private LayerMask _groundMask;
    private Transform _ragdollLegPos;
    private Transform _ragdollOtherLegPos;
    private StaticBoneFollower[] _currentLegBones;
    private CinemachineBasicMultiChannelPerlin _cameraShake;

    public Rigidbody UpperLegRB { get; private set; }
    public Rigidbody LowerLegRB { get; private set; }
    public Rigidbody FeetRB { get; private set; }

    public ConfigurableJoint UpperLegJoint { get; private set; }
    public ConfigurableJoint LowerLegJoint { get; private set; }
    public ConfigurableJoint FeetJoint { get; private set; }
    public ConfigurableJoint HipJoint { get; private set; }

    private float upperLegMass, lowerLegMass, FeetMass;
    private float ragdollDistance = 1.6f;
    public LegInteractionContext(TwoBoneIKConstraint ikConstraint, MultiRotationConstraint legMultiRotationConstraint,
            /*Rigidbody rigidbody, CapsuleCollider rootCollider,*/ Transform legRestPosition, Transform legUpPosition,
            Transform feetRestPosition, Transform legIKTargetTransform, Transform feetIKTargetTransform, BoxCollider legCollider,
            Transform hipPosition, Transform hipNormalPos, LayerMask groundMask, Transform otherLegPosition, Transform fullBodyPos,
            StaticBoneFollower[] currentLegBones, bool isGrounded,
            LegStateMachine.ELegType legType, InputHandler inputHandler, Rigidbody upLegRB, Rigidbody lowLegRB, Rigidbody feetRB,
            ConfigurableJoint upLegJoint, ConfigurableJoint lowLegJoint, ConfigurableJoint feetJoint, Transform ragdollLegPos,
            Transform ragdollOtherLegPos, CinemachineBasicMultiChannelPerlin cameraShake, ConfigurableJoint hipJoint)
    {
        _ikConstraint = ikConstraint;
        _multiRotationConstraint = legMultiRotationConstraint;
        //_rigidbody = rigidbody;
        //_rootCollider = rootCollider;
        _legCollider = legCollider;
        _legRestPosition = legRestPosition;
        _legUpPosition = legUpPosition;
        _feetRestPosition = feetRestPosition;
        _hipPosition = hipPosition;
        _hipNormalPos = hipNormalPos;
        _legIKTargetTransform = legIKTargetTransform;
        _feetIKTargetTransform = feetIKTargetTransform;
        _isGrounded = isGrounded;
        _legType = legType;
        _handler = inputHandler;
        _groundMask = groundMask;
        _otherLegPosition = otherLegPosition;
        _fullBodyPos = fullBodyPos;
        _currentLegBones = currentLegBones;
        UpperLegRB = upLegRB;
        LowerLegRB = lowLegRB;
        FeetRB = feetRB;
        UpperLegJoint = upLegJoint;
        LowerLegJoint = lowLegJoint;
        FeetJoint = feetJoint;
        HipJoint = hipJoint;
        _ragdollLegPos = ragdollLegPos;
        _ragdollOtherLegPos = ragdollOtherLegPos;
        _cameraShake = cameraShake;

        if (UpperLegRB != null) upperLegMass = UpperLegRB.mass;
        if (LowerLegRB != null) lowerLegMass = LowerLegRB.mass;
        if (FeetRB != null) FeetMass = FeetRB.mass;
    }
    public Vector2 CurrentLegMoveValue
    {
        get
        {
            if (_handler == null) return Vector2.zero;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.leftLegMoveValue : _handler.rightLegMoveValue;
        }
    }
    public InputHandler.EControllerInput CurrentLegInput
    {
        get
        {
            if (_handler == null) return InputHandler.EControllerInput.Down;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.LeftLegInput : _handler.RightLegInput;
        }
        set
        {
            if (_handler == null) return;
            if (_legType == LegStateMachine.ELegType.LEFT)
            {
                _handler.LeftLegInput = value;
            }
            else
            {
                _handler.RightLegInput = value;
            }
        }
    }
    public InputHandler.EBodyActiveState LegActiveState
    {
        get
        {
            if (_handler == null) return InputHandler.EBodyActiveState.Down;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.LeftLegActiveState : _handler.RightLegActiveState;
        }
        set
        {
            if (_handler == null) return;

            if (_legType == LegStateMachine.ELegType.LEFT)
            {
                _handler.LeftLegActiveState = value;
            }
            else
            {
                _handler.RightLegActiveState = value;
            }
        }

    }
    public InputHandler.EBodyActiveState OtherLegActiveState
    {
        get
        {
            if (_handler == null) return InputHandler.EBodyActiveState.Down;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.RightLegActiveState : _handler.LeftLegActiveState;
        }
    }
    public void SetLegStiffness(float spring, float damper)
    {
        if (_currentLegBones == null) return;

        foreach (var muscle in _currentLegBones)
        {
            muscle.SetMuscleStrength(spring, damper);
        }
    }
    public void SetLegToIdle()
    {
        // 1. Restaurar masas pesadas
        if (UpperLegRB != null) UpperLegRB.mass = upperLegMass;
        if (LowerLegRB != null) LowerLegRB.mass = lowerLegMass;
        if (FeetRB != null) FeetRB.mass = FeetMass;

        // 2. Fuerza fuerte y estable
        SetJointSlerp(UpperLegJoint, 15000f, 500f);
        SetJointSlerp(LowerLegJoint, 10000f, 300f);
        SetJointSlerp(FeetJoint, 5000f, 200f);
    }

    public void SetLegToRise()
    {
        if (UpperLegRB != null) UpperLegRB.mass = 0.2f;
        if (LowerLegRB != null) LowerLegRB.mass = 0.1f;
        if (FeetRB != null) FeetRB.mass = 0.05f;

        SetJointSlerp(UpperLegJoint, 25000f, 50f);
        SetJointSlerp(LowerLegJoint, 15000f, 30f);
        SetJointSlerp(FeetJoint, 2000f, 10f);
    }

    public void SetJointSlerp(ConfigurableJoint joint, float spring, float damper)
    {
        if (joint == null) return;
        JointDrive drive = joint.slerpDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        joint.slerpDrive = drive;
    }
    public TwoBoneIKConstraint CurrentIkConstraint => _ikConstraint;
    public MultiRotationConstraint CurrentMultiRotationConstraint => _multiRotationConstraint;
    //public Rigidbody Rb => _rigidbody;
    //public CapsuleCollider RootCollider => _rootCollider;
    public Transform LegRestPosition => _legRestPosition;
    public Transform LegUpPosition => _legUpPosition;
    public Transform feetRestPosition => _feetRestPosition;
    public InputHandler InputHandler => _handler;
    public Transform LegIKTargetTransform => _legIKTargetTransform;
    public Transform FeetIKTargetTransform => _feetIKTargetTransform;
    public ELegType LegType => _legType;
    public BoxCollider LegCollider => _legCollider;
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public LayerMask GroundMask => _groundMask;
    public Transform OtherLegPosition => _otherLegPosition;
    public Transform FullBodyPos => _fullBodyPos;
    public Transform HipPosition => _hipPosition;
    public Transform HipNormalPos => _hipNormalPos;
    public StaticBoneFollower[] CurrentLegBones => _currentLegBones;
    public Transform RagdollLegPos => _ragdollLegPos;
    public Transform OtherRagdollLegPos => _ragdollOtherLegPos;
    public CinemachineBasicMultiChannelPerlin CameraShake => _cameraShake;
    public float RagdollDistance => ragdollDistance;
}
