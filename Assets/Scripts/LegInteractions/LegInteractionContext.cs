using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LegInteractionContext
{
    public enum ELegType
    {
        RIGHT,
        LEFT,
        BOTH,
    }
    private InputHandler _handler;
    private TwoBoneIKConstraint _leftIkConstraint;
    private TwoBoneIKConstraint _rightIkConstraint;
    private MultiRotationConstraint _leftMultiRotationConstraint;
    private MultiRotationConstraint _rightMultiRotationConstraint;
    private Rigidbody _rigidbody;
    private CapsuleCollider _rootCollider;
    private Transform _leftLegRestPosition;
    private Transform _leftLegUpPosition;
    private Transform _rightLegRestPosition;
    private Transform _rightLegUpPosition;
    private Transform _rightFeetRestPosition;
    private Transform _leftFeetRestPosition;

    private Transform _rightLegIKTargetTransform;
    private Transform _leftLegIKTargetTransform;
    private Transform _leftFeetIKTargetTransform;
    private Transform _rightFeetTargetIKTransform;
    public ELegType _currentLeg;
    public LegInteractionContext(TwoBoneIKConstraint leftLegIkConstraint, TwoBoneIKConstraint rightLegIkConstraint, 
        MultiRotationConstraint leftLegMultiRotationConstraint, MultiRotationConstraint rightLegMultiRotationConstraint, 
        Rigidbody rigidbody, CapsuleCollider rootCollider, Transform leftLegRestPosition, Transform leftLegUpPosition,
        Transform rightLegRestPosition, Transform rightLegUpPosition, Transform leftFeetRestPosition, Transform rightFeetRestPosition, 
        InputHandler inputHandler, Transform rightLegIKTargetTransform, Transform leftLegIKTargetTransform, Transform leftFeetIKTargetTransform, 
        Transform rightFeetIKTargetTransform)
    {
        _leftIkConstraint = leftLegIkConstraint;
        _rightIkConstraint = rightLegIkConstraint;
        _leftMultiRotationConstraint = leftLegMultiRotationConstraint;
        _rightMultiRotationConstraint = rightLegMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _leftLegRestPosition = leftLegRestPosition;
        _leftLegUpPosition = leftLegUpPosition;
        _rightLegRestPosition = rightLegRestPosition;
        _rightLegUpPosition = rightLegUpPosition;
        _handler = inputHandler;
        _leftLegIKTargetTransform = leftLegIKTargetTransform;
        _rightLegIKTargetTransform = rightLegIKTargetTransform;
        _leftFeetIKTargetTransform = leftFeetIKTargetTransform;
        _rightFeetTargetIKTransform = rightFeetIKTargetTransform;
    }

    public TwoBoneIKConstraint LeftIkConstraint => _leftIkConstraint;
    public TwoBoneIKConstraint RightIkConstraint => _rightIkConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint RightMultiRotationConstraint => _rightMultiRotationConstraint;
    public Rigidbody Rb => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform LeftRestPosition => _leftLegRestPosition;
    public Transform LeftUpPosition => _leftLegUpPosition;
    public Transform RightRestPosition => _rightLegRestPosition;
    public Transform RightUpPosition => _rightLegUpPosition;
    public Transform LeftFeetRestPosition => _leftFeetRestPosition;
    public Transform RightFeetRestPosition => _rightFeetRestPosition;
    public InputHandler InputHandler => _handler; 
    public Transform LeftLegIKTargetTransform => _leftLegIKTargetTransform;
    public Transform RightLegIKTragetTransform => _rightLegIKTargetTransform;
    public Transform LeftFeetIKTargetTransform => _leftFeetIKTargetTransform;
    public Transform RightFeetIKTragetTransform => _rightFeetTargetIKTransform;
    //public TwoBoneIKConstraint CurrentIkConstraint { get; set; }
    //public Transform LeftLegIKTargetTransform {  get; set; }
    //public Transform RightLegIKTragetTransform { get ; set; }
    //public Transform CurrentLegIKTargetTransform { get; set; }
    public ELegType CurrentLeg => _currentLeg;


}
