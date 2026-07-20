using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class LegInteractionStateMachine : StateManager<LegInteractionStateMachine.ELegInteractionState>
{
    public enum ELegInteractionState
    {
        Idle,
        Rise,
        Move,
        Release,
        Touch,
        Relocate,
        Reset,
    }
    private LegInteractionContext _context;
    [SerializeField] private InputHandler _handler;
    [SerializeField] private TwoBoneIKConstraint _leftLegIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightLegIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftLegMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightLegMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;
    [SerializeField] private Transform _leftLegRestPosition;
    [SerializeField] private Transform _leftUpLegPosition;
    [SerializeField] private Transform _rightLegRestPosition;
    [SerializeField] private Transform _rightLegUpPosition;
    [SerializeField] private Transform _leftFeetRestPosition;
    [SerializeField] private Transform _rightFeetRestPosition;

    [SerializeField] private Transform _rightLegIKTransform;
    [SerializeField] private Transform _leftLegIKTransform;
    [SerializeField] private Transform _leftFeetIKTransform;
    [SerializeField] private Transform _leftRightIKTransform;
    
    private void Awake()
    {
        ValidateConstraints();
        _handler = GetComponent<InputHandler>();
        _context = new LegInteractionContext(_leftLegIkConstraint, _rightLegIkConstraint, _leftLegMultiRotationConstraint, _rightLegMultiRotationConstraint,
            _rigidbody, _rootCollider,_leftLegRestPosition,_leftUpLegPosition,_rightLegRestPosition,_rightLegUpPosition,
            _leftFeetRestPosition, _rightFeetRestPosition,_handler);
        InitializeStates();
    }
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftLegIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightLegIkConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_rightLegMultiRotationConstraint, "Right multi rotation constraint is not assigned.");
        Assert.IsNotNull(_leftLegMultiRotationConstraint, "Left multi rotation constraint is not assigned.");
        Assert.IsNotNull(_rootCollider, "root Collider is not assigned.");
        Assert.IsNotNull(_rigidbody, "rigidbody is not assigned.");
        Assert.IsNotNull(_handler, "Input Handler is not assigned.");

    }
    private void InitializeStates()
    {
        States.Add(ELegInteractionState.Idle, new IdleLegState(_context, ELegInteractionState.Idle));
        States.Add(ELegInteractionState.Rise, new RiseState(_context, ELegInteractionState.Rise));
        States.Add(ELegInteractionState.Move, new MovingState(_context, ELegInteractionState.Move));
        States.Add(ELegInteractionState.Release, new ReleaseState(_context, ELegInteractionState.Release));
        States.Add(ELegInteractionState.Touch, new TouchState(_context, ELegInteractionState.Touch));
        States.Add(ELegInteractionState.Relocate, new RelocateState(_context, ELegInteractionState.Relocate));
        States.Add(ELegInteractionState.Reset, new ResetState(_context, ELegInteractionState.Reset));
        currentState = States[ELegInteractionState.Reset];
    }

}
