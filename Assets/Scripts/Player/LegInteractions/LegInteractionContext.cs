using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static LegStateMachine;

public class LegInteractionContext
{
    private InputHandler _handler;
    private TwoBoneIKConstraint _ikConstraint;
    private MultiRotationConstraint _multiRotationConstraint;
    private BoxCollider _legCollider;

    private Transform _fullBodyPos;
    private Transform _legRestPosition;
    private Transform _legUpPosition;
    private Transform _hipPosition;
    private Transform _otherLegPosition;

    private Transform _legIKTargetTransform;
    private LegStateMachine.ELegType _legType;
    private bool _isGrounded;
    private InputHandler.EControllerInput _currentLegInput;
    public Vector3 _currentLegPos;
    private LayerMask _groundMask;
    private Transform _ragdollLegPos;
    private Transform _ragdollOtherLegPos;
    private StaticBoneFollower[] _currentLegBones;
    private CinemachineBasicMultiChannelPerlin _cameraShake;
    private GrowManager _growManager;
    public Rigidbody UpperLegRB { get; private set; }
    public Rigidbody LowerLegRB { get; private set; }
    public Rigidbody FeetRB { get; private set; }

    public ConfigurableJoint UpperLegJoint { get; private set; }
    public ConfigurableJoint LowerLegJoint { get; private set; }
    public ConfigurableJoint FeetJoint { get; private set; }
    public ConfigurableJoint HipJoint { get; private set; }
    public Rigidbody _hipRB;

    private float upperLegMass, lowerLegMass, FeetMass;
    private float ragdollDistance = 1.5f;
    private float movingSpeed = 2.5f;
    private float maxLegMovingDistance = .9f;
    private float currentScale =1f;

    public float CurrentScale => _growManager != null ? _growManager.CurrentScale : 1f;

    public float MaxLegMovingDistance
    {
        get { return maxLegMovingDistance * CurrentScale; }
        set { maxLegMovingDistance = value; }
    }
    public float MovingSpeed
    {
        get { return movingSpeed * CurrentScale; }
        set { movingSpeed = value; }
    }
    public float RagdollDistance
    {
        get { return ragdollDistance * CurrentScale; }
        set { ragdollDistance = value; }
    }
    
    public LegInteractionContext(TwoBoneIKConstraint ikConstraint, MultiRotationConstraint legMultiRotationConstraint,
            /*Rigidbody rigidbody, CapsuleCollider rootCollider,*/ Transform legRestPosition, Transform legUpPosition,
            Transform legIKTargetTransform, BoxCollider legCollider,
            Transform hipPosition, LayerMask groundMask, Transform otherLegPosition, Transform fullBodyPos,
            StaticBoneFollower[] currentLegBones, bool isGrounded,
            LegStateMachine.ELegType legType, InputHandler inputHandler, Rigidbody upLegRB, Rigidbody lowLegRB, Rigidbody feetRB,
            ConfigurableJoint upLegJoint, ConfigurableJoint lowLegJoint, ConfigurableJoint feetJoint, Transform ragdollLegPos,
            Transform ragdollOtherLegPos, CinemachineBasicMultiChannelPerlin cameraShake, ConfigurableJoint hipJoint, Rigidbody hipRB, GrowManager growManager)
    {
        _ikConstraint = ikConstraint;
        _multiRotationConstraint = legMultiRotationConstraint;
        //_rigidbody = rigidbody;
        //_rootCollider = rootCollider;
        _legCollider = legCollider;
        _legRestPosition = legRestPosition;
        _legUpPosition = legUpPosition;
        _hipPosition = hipPosition;
        _legIKTargetTransform = legIKTargetTransform;
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
        _hipRB = hipRB;
        _growManager = growManager;

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
        float scale = CurrentScale;

        // 1. Restaurar masas escaladas al tamaño actual
        if (UpperLegRB != null) UpperLegRB.mass = upperLegMass * scale;
        if (LowerLegRB != null) LowerLegRB.mass = lowerLegMass * scale;
        if (FeetRB != null) FeetRB.mass = FeetMass * scale;

        // 2. Fuerza fuerte y estable
        SetJointSlerp(UpperLegJoint, _growManager.GetJointSpring(UpperLegJoint), _growManager.GetJointDamper(UpperLegJoint));
        SetJointSlerp(LowerLegJoint, _growManager.GetJointSpring(LowerLegJoint), _growManager.GetJointDamper(LowerLegJoint));
        SetJointSlerp(FeetJoint, _growManager.GetJointSpring(FeetJoint), _growManager.GetJointDamper(FeetJoint));
    }

    public void SetLegToRise()
    {
        float scale = CurrentScale;

        if (UpperLegRB != null) UpperLegRB.mass = 0.2f * scale;
        if (LowerLegRB != null) LowerLegRB.mass = 0.1f * scale;
        if (FeetRB != null) FeetRB.mass = 0.05f * scale;

        SetJointSlerp(UpperLegJoint, _growManager.GetJointSpring(UpperLegJoint), _growManager.GetJointDamper(UpperLegJoint));
        SetJointSlerp(LowerLegJoint, _growManager.GetJointSpring(LowerLegJoint), _growManager.GetJointDamper(LowerLegJoint));
        SetJointSlerp(FeetJoint, _growManager.GetJointSpring(FeetJoint), _growManager.GetJointDamper(FeetJoint));
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
    public Transform LegRestPosition => _legRestPosition;
    public Transform LegUpPosition => _legUpPosition;
    public InputHandler InputHandler => _handler;
    public Transform LegIKTargetTransform => _legIKTargetTransform;
    public ELegType LegType => _legType;
    public BoxCollider LegCollider => _legCollider;
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public LayerMask GroundMask => _groundMask;
    public Transform OtherLegPosition => _otherLegPosition;
    public Transform FullBodyPos => _fullBodyPos;
    public Transform HipPosition => _hipPosition;
    public StaticBoneFollower[] CurrentLegBones => _currentLegBones;
    public Transform RagdollLegPos => _ragdollLegPos;
    public Rigidbody HipRB => _hipRB;
    public Transform OtherRagdollLegPos => _ragdollOtherLegPos;
    public CinemachineBasicMultiChannelPerlin CameraShake => _cameraShake;
}
