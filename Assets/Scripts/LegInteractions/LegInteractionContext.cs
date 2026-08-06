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
    private InputHandler.ELegInput _currentLegState;
    public Vector3 _currentLegPos;
    private LayerMask _groundMask;
    public LegInteractionContext(TwoBoneIKConstraint ikConstraint, MultiRotationConstraint legMultiRotationConstraint, 
        /*Rigidbody rigidbody, CapsuleCollider rootCollider,*/ Transform legRestPosition, Transform legUpPosition,
        Transform feetRestPosition, Transform legIKTargetTransform, Transform feetIKTargetTransform, BoxCollider legCollider, 
        Transform hipPosition, Transform hipNormalPos, LayerMask groundMask,Transform otherLegPosition,Transform fullBodyPos, bool isGrounded,
        LegStateMachine.ELegType legType, InputHandler inputHandler)
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

    }
    public Vector2 CurrentLegMoveValue
    {
        get
        {
            if (_handler == null) return Vector2.zero;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.leftLegMoveValue : _handler.rightLegMoveValue;
        }
    }
    public InputHandler.ELegInput CurrentLegState
    {
        get
        {
            if (_handler == null) return InputHandler.ELegInput.Down;
            return _legType == LegStateMachine.ELegType.LEFT ? _handler.LeftLegState : _handler.RightLegState;
        }
    }
    public TwoBoneIKConstraint LeftIkConstraint => _ikConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => _multiRotationConstraint;
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
}
