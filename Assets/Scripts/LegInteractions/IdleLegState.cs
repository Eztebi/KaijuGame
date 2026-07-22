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
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if(Context.LegType == LegStateMachine.ELegType.LEFT)
        {
            if (Context.InputHandler.LeftLegState == InputHandler.LegState.Up)
            {
                Debug.Log("Rise State LEFT");
                return LegStateMachine.ELegState.Rise;
            }    
        }
        if(Context.LegType == LegStateMachine.ELegType.RIGHT)
        {
            if (Context.InputHandler.RightLegState == InputHandler.LegState.Up)
            {
                Debug.Log("Rise State RIGHT");
                return LegStateMachine.ELegState.Rise;
            }    
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