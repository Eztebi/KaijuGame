using UnityEngine;

public class DebugMoveLegState : LegInteractionState
{
    public DebugMoveLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {

    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {

    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Up) return LegStateMachine.ELegState.Reset;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision)
    {
    }
    public override void OnCollisionStay(Collision collision)
    {
    }
    public override void OnCollisionExit(Collision collision)
    {
    }

}
