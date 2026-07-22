using UnityEngine;

public class IdleLegState : LegInteractionState
{
    public IdleLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        //Context.i
        Debug.Log("Enter Idle State");
    }
    public override void ExitState() {
        
    }
    public override void UpdateState() {
        Debug.Log("Update Idle State");
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if(Context.CurrentLegState == InputHandler.ELegPos.Up)
        {
            return LegStateMachine.ELegState.Rise;
        }
        return CheckToReturnToReset();
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    public override void MoveToPosition()
    {
        Context.LegIKTargetTransform.position = Context.LegRestPosition.position;
        Context._currentLegPos = Context.LegRestPosition.position;
    }
}