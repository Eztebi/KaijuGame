using UnityEngine;

public class ReleaseState : LegInteractionState
{
    public ReleaseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Release State");

    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Release State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        return CheckToReturnToReset();
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}
