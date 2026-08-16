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
    public override void EnterState()
    {
        Context.SetLegToIdle();
        Context.LegActiveState = InputHandler.ELegActiveState.Down;
        //Context._currentLegPos = Context.LegRestPosition.position;
        //Context.LegIKTargetTransform.position = Context._currentLegPos;

        //Context.HipJoint.xMotion = ConfigurableJointMotion.Locked;
        //Context.HipJoint.zMotion = ConfigurableJointMotion.Locked;
        //Context.HipJoint.yMotion = ConfigurableJointMotion.Locked;
        //LegBoneMotionType(ConfigurableJointMotion.Locked);
        Debug.Log("Enter Idle State");
        Context.LegIKTargetTransform.position = Context._currentLegPos;
        //MoveToPosition();
    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
       // Context.LegIKTargetTransform.position = Context._currentLegPos;
        Debug.Log("Update Idle State");
        Context.LegIKTargetTransform.position = Context._currentLegPos;
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.CurrentLegInput == InputHandler.ELegInput.Up)
        {
            return LegStateMachine.ELegState.Rise;
        }
        //if (Context.LegActiveState == InputHandler.ELegActiveState.Ragdoll) return LegStateMachine.ELegState.Ragdoll;
        //return CheckToReturnToReset();
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
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
    public override void OnCollisionExit(Collision collision) { }

}
    