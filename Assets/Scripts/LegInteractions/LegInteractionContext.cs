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
    private InputHandler.ELegInput _currentLegInput;
    private LayerMask _groundMask;
    public Vector3 CurrentLegPos;
    private Transform _ragdollOtherLegPos;

    public Rigidbody UpperLegRB { get; private set; }
    public Rigidbody LowerLegRB { get; private set; }
    public Rigidbody FeetRB { get; private set; }

    public ConfigurableJoint UpperLegJoint { get; private set; }
    public ConfigurableJoint LowerLegJoint { get; private set; }
    public ConfigurableJoint FeetJoint { get; private set; }

    private float upperLegMass, lowerLegMass, FeetMass;

    public LegInteractionContext(TwoBoneIKConstraint ikConstraint, MultiRotationConstraint legMultiRotationConstraint,
            /*Rigidbody rigidbody, CapsuleCollider rootCollider,*/ Transform legRestPosition, Transform legUpPosition,
            Transform feetRestPosition, Transform legIKTargetTransform, Transform feetIKTargetTransform, BoxCollider legCollider,
            Transform hipPosition, Transform hipNormalPos, LayerMask groundMask, Transform otherLegPosition, Transform fullBodyPos,
            bool isGrounded,LegStateMachine.ELegType legType, InputHandler inputHandler, Rigidbody upLegRB, Rigidbody lowLegRB, Rigidbody feetRB,
            ConfigurableJoint upLegJoint, ConfigurableJoint lowLegJoint, ConfigurableJoint feetJoint, Vector3 currentLegPos,
            Transform ragdollOtherLegPos)
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
        UpperLegRB = upLegRB;
        LowerLegRB = lowLegRB;
        FeetRB = feetRB;
        UpperLegJoint = upLegJoint;
        LowerLegJoint = lowLegJoint;
        FeetJoint = feetJoint;
        CurrentLegPos = currentLegPos;
        _ragdollOtherLegPos = ragdollOtherLegPos;

        if (UpperLegRB != null) upperLegMass = UpperLegRB.mass;
        if (LowerLegRB != null) lowerLegMass = LowerLegRB.mass;
        if (FeetRB != null) FeetMass = FeetRB.mass;
    }
    public Vector2 CurrentLegMoveValue
    {
        get
        {
            if (_handler == null) return Vector2.zero;
            return _handler.legMoveValue;
        }
    }
    public InputHandler.ELegInput CurrentLegInput
    {
        get
        {
            if (_handler == null) return InputHandler.ELegInput.Down;
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
    public InputHandler.ELegActiveState LegActiveState
    {
        get
        {
            if (_handler == null) return InputHandler.ELegActiveState.Down;
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
    public InputHandler.ELegActiveState OtherLegActiveState
    {
        get
        {
            if (_handler == null) return InputHandler.ELegActiveState.Down;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.RightLegActiveState : _handler.LeftLegActiveState;
        }
    }

    private void SetJointSlerp(ConfigurableJoint joint, float spring, float damper)
    {
        if (joint == null) return;
        JointDrive drive = joint.slerpDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        joint.slerpDrive = drive;
    }
    public void SetLegStiffness(float spring, float damper)
    {
        SetJointSlerp(UpperLegJoint, spring, damper);
        SetJointSlerp(LowerLegJoint, spring, damper);
        SetJointSlerp(FeetJoint, spring, damper);
    }
    public void SetIkWeight(float weight)
    {
        if (_ikConstraint != null) _ikConstraint.weight = weight;
        if (_multiRotationConstraint != null) _multiRotationConstraint.weight = weight;
    }
    public void SetLegToIdle()
    {
        SetIkWeight(1f); // Aseguramos que el IK controle la pose animada

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
        SetIkWeight(1f); // Aseguramos que el IK esté activo

        // 1. Pierna liviana para que no colapse la cadera
        if (UpperLegRB != null) UpperLegRB.mass = 0.2f;
        if (LowerLegRB != null) LowerLegRB.mass = 0.1f;
        if (FeetRB != null) FeetRB.mass = 0.05f;

        // 2. Músculos hiper-reactivos para seguir al IK al instante
        SetJointSlerp(UpperLegJoint, 25000f, 50f);
        SetJointSlerp(LowerLegJoint, 15000f, 30f);
        SetJointSlerp(FeetJoint, 5000f, 10f);
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
    public Transform OtherRagdollLegPos => _ragdollOtherLegPos;
}
