using UnityEngine;
using UnityEngine.Animations.Rigging;
using static LegStateMachine;

public class LegInteractionContext
{
    private InputHandler _handler;
    private TwoBoneIKConstraint _ikConstraint;
    private MultiRotationConstraint _multiRotationConstraint;
    private Rigidbody _rigidbody;
    private CapsuleCollider _rootCollider;

    private Transform _legRestPosition;
    private Transform _legUpPosition;
    private Transform _feetRestPosition;

    private Transform _legIKTargetTransform;
    private Transform _feetIKTargetTransform;
    private LegStateMachine.ELegType _legType;
    private bool _isGrounded;

    public LegInteractionContext(TwoBoneIKConstraint ikConstraint, MultiRotationConstraint legMultiRotationConstraint, 
        Rigidbody rigidbody, CapsuleCollider rootCollider, Transform legRestPosition, Transform legUpPosition,
        Transform feetRestPosition, Transform legIKTargetTransform, Transform feetIKTargetTransform, bool isGrounded,
        LegStateMachine.ELegType legType, InputHandler inputHandler)
    {
        _ikConstraint = ikConstraint;
        _multiRotationConstraint = legMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _legRestPosition = legRestPosition;
        _legUpPosition = legUpPosition;
        _feetRestPosition = feetRestPosition;
        _legIKTargetTransform = legIKTargetTransform;
        _feetIKTargetTransform = feetIKTargetTransform;
        _isGrounded = isGrounded;
        _legType = legType;
        _handler = inputHandler;
    }

    public TwoBoneIKConstraint LeftIkConstraint => _ikConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => _multiRotationConstraint;
    public Rigidbody Rb => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform LegRestPosition => _legRestPosition;
    public Transform LegUpPosition => _legUpPosition;
    public Transform feetRestPosition => _feetRestPosition;
    public InputHandler InputHandler => _handler; 
    public Transform LegIKTargetTransform => _legIKTargetTransform;
    public Transform FeetIKTargetTransform => _feetIKTargetTransform;
    public ELegType LegType => _legType;

    //public TwoBoneIKConstraint CurrentIkConstraint { get; set; }
    //public Transform LeftLegIKTargetTransform {  get; set; }
    //public Transform RightLegIKTragetTransform { get ; set; }
    //public Transform CurrentLegIKTargetTransform { get; set; }
}
