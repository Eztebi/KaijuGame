using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class IdleLegState : LegInteractionState
{
    public IdleLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        //LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        //Context.i
        Context.LegIKTargetTransform.position = Context._currentLegPos;
        Debug.Log("Enter Idle State");
        //MoveToPosition();
    }
    public override void ExitState() {
        
    }
    public override void UpdateState() {
        Debug.Log("Update Idle State");
        
        Context.LegIKTargetTransform.position = Context._currentLegPos;
        
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if(Context.CurrentLegState == InputHandler.ELegInput.Up)
        {
            return LegStateMachine.ELegState.Rise;

        }
        //return CheckToReturnToReset();
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Context.IsGrounded = true;
        }
    }
    public override void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Context.IsGrounded = true;
        }
    }
    public override void OnCollisionExit(Collision collision) { }
    
}