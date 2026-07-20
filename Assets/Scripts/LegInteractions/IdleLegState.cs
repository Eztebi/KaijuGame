using UnityEngine;

public class IdleLegState : LegInteractionState
{
    public IdleLegState(LegInteractionContext context, LegInteractionStateMachine.ELegInteractionState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Idle State");
    }
    public override void ExitState() {
        
    }
    public override void UpdateState() {

        Debug.Log("Update Idle State");
    }
    public override LegInteractionStateMachine.ELegInteractionState GetNextState()
    {
        if (Context.InputHandler.LeftLegState == InputHandler.LegState.Up)
        {
            Debug.Log("Rise State no entraa");
            Context.CurrentLeg = LegInteractionContext.ELegType.LEFT;
            return LegInteractionStateMachine.ELegInteractionState.Rise;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}