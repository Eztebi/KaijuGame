using UnityEngine;

public class MovingState : LegInteractionState
{
    public MovingState(LegInteractionContext context, LegInteractionStateMachine.ELegInteractionState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() { }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override LegInteractionStateMachine.ELegInteractionState GetNextState()
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
