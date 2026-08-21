using Unity.Cinemachine;
using UnityEngine;
using static LegStateMachine;

public class BodyStateMachine : StateManager<BodyStateMachine.EBodyState>
{
    public enum EBodyState
    {
        Idle,
        Ragdoll,
        Falling,
        StandingUp,
        Reset
    }
    private BodyStateMachineContext _context;

    [SerializeField] private InputHandler _handler;
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bodyPos;
    [SerializeField] private StaticBoneFollower[] _bones;

    [SerializeField] private Rigidbody _hipsRB;
    [SerializeField] private Rigidbody _spineRB;
    [SerializeField] private Rigidbody _headRB;

    [SerializeField] private ConfigurableJoint _hipsJoint;
    [SerializeField] private ConfigurableJoint _spineJoint;
    [SerializeField] private ConfigurableJoint _headJoint;
    [SerializeField] private CinemachineOrbitalFollow _cinemachineOrbitalFollow;

    [SerializeField] private Transform _rightLegPos;
    [SerializeField] private Transform _leftLegPos;
    [SerializeField] private Transform _SpinePos;
    private void Awake()
    {
        ValidateConstraints();
        _handler = GetComponent<InputHandler>();
        _context = new BodyStateMachineContext(_bodyPos, _bones,  _handler, _rigidbodies, _animator, _hipsRB, _spineRB, _headRB, 
            _hipsJoint, _spineJoint, _headJoint, _rightLegPos, _leftLegPos, _SpinePos);
        InitializeStates();
    }
    private void ValidateConstraints()
    {
        

    }
    private void InitializeStates()
    {
        States.Add(EBodyState.Idle, new IdleBodyState(_context, EBodyState.Idle));
        States.Add(EBodyState.Ragdoll, new RagdollBodyState(_context, EBodyState.Ragdoll));
        States.Add(EBodyState.StandingUp, new StandingUpBodyState(_context, EBodyState.StandingUp));
        States.Add(EBodyState.Reset, new ResetBodyState(_context, EBodyState.Reset));

        currentState = States[EBodyState.Reset];
    }
   
}
