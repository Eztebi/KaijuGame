using UnityEngine;

public class RiseState : LegInteractionState
{
    public RiseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context,estate) {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Rise State");
        MoveToPosition();
    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Rise State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    private void MoveToPosition()
    {
        Context.LegIKTargetTransform.position = Context.LegUpPosition.position;
    }
}
