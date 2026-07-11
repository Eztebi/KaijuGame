using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class LegInteractionStateMachine : StateManager<LegInteractionStateMachine.ELegInteractionState>
{
    public enum ELegInteractionState
    {
        Relocate,
        Rise,
        Move,
        Release,
        Touch,
        Reset
    }

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi rotation constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi rotation constraint is not assigned.");
        Assert.IsNotNull(_rootCollider, "root Collider is not assigned.");
        Assert.IsNotNull(_rigidbody, "rigidbody is not assigned.");

    }
}
