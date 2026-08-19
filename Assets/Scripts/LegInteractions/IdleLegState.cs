using UnityEngine;

public class IdleLegState : LegInteractionState
{
    private float moveSpeed = .1f;

    public IdleLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState()
    {
        Context.SetLegToIdle();
        Context.LegActiveState = InputHandler.ELegActiveState.Down;

        Debug.Log("Enter Idle State");
        Context.LegIKTargetTransform.position = Context.CurrentLegPos;
    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
       // Context.LegIKTargetTransform.position = Context._currentLegPos;
        Debug.Log("Update Idle State");
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.CurrentLegInput == InputHandler.ELegInput.Up)
        {
            return LegStateMachine.ELegState.Rise;
        }
        if(Context.OtherLegActiveState == InputHandler.ELegActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
        }
        //if (Context.LegActiveState == InputHandler.ELegActiveState.Ragdoll) return LegStateMachine.ELegState.Ragdoll;
        //return CheckToReturnToReset();
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) {
        
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Context.IsGrounded = true;
        }
    }
    public override void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Context.IsGrounded = true;
        }
    }
    public override void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("Ground") && !Context.IsGrounded)
        {
            Debug.LogWarning("Choco con suelo");
            Context.IsGrounded = false;
        }
    }
    public override void MoveToPosition()
    {
        Context.CurrentLegPos = Vector3.Lerp(Context.CurrentLegPos, GetFloorPosition(), Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Context.CurrentLegPos;
    }

    private Vector3 GetFloorPosition()
    {
        return Context.LegRestPosition.position;
    }

}
    