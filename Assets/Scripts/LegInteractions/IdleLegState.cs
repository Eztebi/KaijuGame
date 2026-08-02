using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
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
        //MoveToPosition();
    }
    public override void ExitState() {
        
    }
    public override void UpdateState() {
        Debug.Log("Update Idle State");
        //MoveToPosition();
        FeetRestPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if(Context.CurrentLegState == InputHandler.ELegPos.Up)
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
    public override void MoveToPosition()
    {
        Context.LegIKTargetTransform.position = Context.LegRestPosition.position;
        Context._currentLegPos = Context.LegRestPosition.position;
    }
    private void FeetRestPosition()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x,Context.BodyPosition.position.y,Context._currentLegPos.z);
        float feetHeight = Context.BodyPosition.position.y - Context.LegRestPosition.position.y;
        Ray ray = new Ray(pos, Vector3.down * feetHeight);
        if(Physics.Raycast(ray, out RaycastHit hit, feetHeight,Context.GroundMask))
        {
            Context.LegIKTargetTransform.position = hit.point;
            Context._currentLegPos = hit.point;
        }
    }
    
}