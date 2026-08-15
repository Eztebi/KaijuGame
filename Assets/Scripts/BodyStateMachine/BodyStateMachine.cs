using UnityEngine;
using static LegStateMachine;
using static UnityEditor.VersionControl.Asset;

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
    private void Awake()
    {
        ValidateConstraints();
        _handler = GetComponent<InputHandler>();
        _context = new BodyStateMachineContext(_bodyPos, _bones,  _handler, _rigidbodies, _animator);
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
