 using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class LegInteractionStateMachine : StateManager<LegInteractionStateMachine.ELegInteractionState>
{
    public enum ELegInteractionState
    {
        Rise,
        Move,
        Release,
        Touch,
        Relocate,
        Reset,
    }
    private LegInteractionContext _context;
    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void Awake()
    {
        ValidateConstraints();

        _context = new LegInteractionContext(_leftIkConstraint, _rightIkConstraint, _leftMultiRotationConstraint, _rightMultiRotationConstraint,
            _rigidbody, _rootCollider);
        InitializeStates();
    }
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi rotation constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi rotation constraint is not assigned.");
        Assert.IsNotNull(_rootCollider, "root Collider is not assigned.");
        Assert.IsNotNull(_rigidbody, "rigidbody is not assigned.");

    }
    private void InitializeStates()
    {
        States.Add(ELegInteractionState.Rise, new RiseState(_context, ELegInteractionState.Rise));
        States.Add(ELegInteractionState.Move, new MovingState(_context, ELegInteractionState.Move));
        States.Add(ELegInteractionState.Release, new ReleaseState(_context, ELegInteractionState.Release));
        States.Add(ELegInteractionState.Touch, new TouchState(_context, ELegInteractionState.Touch));
        States.Add(ELegInteractionState.Relocate, new RelocateState(_context, ELegInteractionState.Relocate));
        States.Add(ELegInteractionState.Reset, new ResetState(_context, ELegInteractionState.Reset));
        currentState = States[ELegInteractionState.Reset];
    }
}
