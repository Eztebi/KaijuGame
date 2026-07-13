using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LegInteractionContext
{
    private TwoBoneIKConstraint _leftIkConstraint;
    private TwoBoneIKConstraint _rightIkConstraint;
    private MultiRotationConstraint _leftMultiRotationConstraint;
    private MultiRotationConstraint _rightMultiRotationConstraint;
    private Rigidbody _rigidbody;
    private CapsuleCollider _rootCollider;

    public LegInteractionContext(TwoBoneIKConstraint leftIkConstraint, TwoBoneIKConstraint rightIkConstraint, MultiRotationConstraint leftMultiRotationConstraint, MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider)
    {
        _leftIkConstraint = leftIkConstraint;
        _rightIkConstraint = rightIkConstraint;
        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
    }

    public TwoBoneIKConstraint LeftIkConstraint => _leftIkConstraint;
    public TwoBoneIKConstraint RightIkConstraint => _rightIkConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint RightMultiRotationConstraint => _rightMultiRotationConstraint;
    public Rigidbody Rb => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
}
