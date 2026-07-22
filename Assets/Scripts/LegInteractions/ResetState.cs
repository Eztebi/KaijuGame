using UnityEngine;

public class ResetState : LegInteractionState
{
    public ResetState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Reset State");
    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Reset State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        return LegStateMachine.ELegState.Idle;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}
