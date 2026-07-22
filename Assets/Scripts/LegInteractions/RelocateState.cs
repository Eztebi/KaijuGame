using UnityEngine;

public class RelocateState : LegInteractionState
{
    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Relocate State");
    }
    public override void ExitState() {
        Debug.Log("Update Relocate State");
    }
    public override void UpdateState() { }
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
