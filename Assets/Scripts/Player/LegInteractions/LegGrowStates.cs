using UnityEngine;

public class LegGrowState : LegInteractionState
{
    public LegGrowState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState()
    {
        //Context.RagdollDistance *= Context.CurrentScale; 
        //Context.MaxLegMovingDistance *= Context.CurrentScale; 
        //Context.MovingSpeed *= Context.CurrentScale; 
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        Debug.Log("Growing State");

    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.InputHandler.BodyActiveState != InputHandler.EBodyActiveState.Grow) return LegStateMachine.ELegState.Reset;

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

};
