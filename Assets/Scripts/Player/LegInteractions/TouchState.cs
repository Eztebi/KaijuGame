using UnityEngine;

public class TouchState : LegInteractionState
{
    public TouchState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState()
    {
        Context.SetLegToIdle();

        Context.LegActiveState = InputHandler.EBodyActiveState.Down;
        Context.IsGrounded = true;
        Debug.Log("Enter Touch State");
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        Debug.Log("Update Touch State");

    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Down) return LegStateMachine.ELegState.DebugMode;

        return LegStateMachine.ELegState.Reset;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

};
