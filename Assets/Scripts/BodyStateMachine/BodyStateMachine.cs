using UnityEngine;
using static LegStateMachine;
using static UnityEditor.VersionControl.Asset;

public class BodyStateMachine : StateManager<BodyStateMachine.EBodyState>
{
    public enum EBodyState
    {
        Idle,
        ActivateRagdoll,
        Falling,
        StandingUp,
        Reset
    }
    private BodyStateMachineContext _context;

    [SerializeField] private InputHandler _handler;
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bodyPos;
    private void Awake()
    {
        ValidateConstraints();
        _handler = GetComponent<InputHandler>();
        _context = new BodyStateMachineContext(_bodyPos, _handler, _rigidbodies, _animator);
        InitializeStates();
    }
    private void ValidateConstraints()
    {
        

    }
    private void InitializeStates()
    {
        States.Add(EBodyState.Idle, new IdleBodyState(_context, EBodyState.Idle));
        States.Add(EBodyState.ActivateRagdoll, new ActivateRagdollBodyState(_context, EBodyState.ActivateRagdoll));
        States.Add(EBodyState.Falling, new FallingBodyState(_context, EBodyState.Falling));
        States.Add(EBodyState.StandingUp, new StandingUpBodyState(_context, EBodyState.StandingUp));
        States.Add(EBodyState.Reset, new ResetBodyState(_context, EBodyState.Reset));

        currentState = States[EBodyState.Reset];
    }
   
}
