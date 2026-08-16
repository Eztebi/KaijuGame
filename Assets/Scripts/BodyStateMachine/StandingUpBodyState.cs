using UnityEngine;

public class StandingUpBodyState : BodyState
{
    public StandingUpBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter StandingUp State");
    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
        Debug.Log("Update Standing Up State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}