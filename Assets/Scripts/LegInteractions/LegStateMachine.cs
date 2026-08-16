using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class LegStateMachine : StateManager<LegStateMachine.ELegState>
{
    public enum ELegState
    {
        Idle,
        Rise,
        Move,
        Release,
        Touch,
        Relocate,
        Ragdoll,
        Reset,
    }
    public enum ELegType
    {
        RIGHT,
        LEFT,
    }

    private LegInteractionContext _context;
    [SerializeField] private Transform _fullBody;
    [SerializeField] private Transform _otherLeg;
    [SerializeField] private InputHandler _handler;
    [SerializeField] private TwoBoneIKConstraint _legIkConstraint;
    [SerializeField] private MultiRotationConstraint _legMultiRotationConstraint;
    //[SerializeField] private Rigidbody _rigidbody;
    //[SerializeField] private CapsuleCollider _rootCollider;
    [SerializeField] private BoxCollider _legCollider;
    [SerializeField] private Transform _legRestPosition;
    [SerializeField] private Transform _upLegPosition;
    [SerializeField] private Transform _feetRestPosition;
    [SerializeField] private Transform _hipPosition;
    [SerializeField] private Transform _hipNormalPos;

    [SerializeField] private Transform _legIKTargetTransform;
    [SerializeField] private Transform _feetIKTargetTransform;

    [SerializeField] private ELegType _eLegType;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask _groundLayerMask;

    [SerializeField] private StaticBoneFollower[] _currentLegBones;
    [SerializeField] private ConfigurableJoint _hipJoint;
    [SerializeField] private ConfigurableJoint _spineJoimt;
    [SerializeField] private Rigidbody _hipRB;

    [Header("Fisicas piernas")]
    [SerializeField] private Rigidbody _upperLegRB;
    [SerializeField] private Rigidbody _lowerLegRB;
    [SerializeField] private Rigidbody _feetRB;

    [SerializeField] private ConfigurableJoint _upperLegJoint;
    [SerializeField] private ConfigurableJoint _lowerLegJoint;
    [SerializeField] private ConfigurableJoint _feetJoint;
    private void Awake()
    {
        ValidateConstraints();
        _handler = GetComponent<InputHandler>();
        FeetCollisionMessenger forwarder = _legCollider.gameObject.GetComponent<FeetCollisionMessenger>();
        if (forwarder == null)
        {
            forwarder = _legCollider.gameObject.AddComponent<FeetCollisionMessenger>();
        }
        forwarder.stateMachine = this;

        _context = new LegInteractionContext(_legIkConstraint, _legMultiRotationConstraint,
            /*_rigidbody, _rootCollider,*/ _legRestPosition, _upLegPosition,
            _feetRestPosition, _legIKTargetTransform, _feetIKTargetTransform, _legCollider, _hipPosition, _hipNormalPos,
            _groundLayerMask, _otherLeg, _fullBody,_hipRB, _currentLegBones, _hipJoint, _spineJoimt, isGrounded, _eLegType,
            _handler,_upperLegRB, _lowerLegRB, _feetRB, _upperLegJoint, _lowerLegJoint, _feetJoint);
        InitializeStates();
    }
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_legIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_legMultiRotationConstraint, "Left multi rotation constraint is not assigned.");
        //Assert.IsNotNull(_rootCollider, "root Collider is not assigned.");
        //Assert.IsNotNull(_rigidbody, "rigidbody is not assigned.");
        Assert.IsNotNull(_handler, "Input Handler is not assigned.");

    }
    private void InitializeStates()
    {
        States.Add(ELegState.Idle, new IdleLegState(_context, ELegState.Idle));
        States.Add(ELegState.Rise, new RiseState(_context, ELegState.Rise));
        States.Add(ELegState.Move, new MovingState(_context, ELegState.Move));
        States.Add(ELegState.Release, new ReleaseState(_context, ELegState.Release));
        States.Add(ELegState.Touch, new TouchState(_context, ELegState.Touch));
        States.Add(ELegState.Relocate, new RelocateState(_context, ELegState.Relocate));
        States.Add(ELegState.Reset, new ResetState(_context, ELegState.Reset));
        States.Add(ELegState.Ragdoll, new RagdollLegState(_context, ELegState.Ragdoll));
        currentState = States[ELegState.Reset];
    }
    public bool Isgrounded => isGrounded;
    public ELegType LegType => _eLegType;
    //private void OnDrawGizmos()
    //{
    //    if (_context == null)
    //        return;

    //    Gizmos.color = Color.magenta;

    //    Gizmos.DrawRay(_context._currentLegPos, Vector3.down * (_context.LegUpPosition.position.y - _context.LegRestPosition.position.y));

    //    Gizmos.color = Color.pink;
    //    Gizmos.DrawRay(_hipPosition.position, Vector3.down * (_context.LegUpPosition.position.y - _context.LegRestPosition.position.y) * 2);
    //}
}
