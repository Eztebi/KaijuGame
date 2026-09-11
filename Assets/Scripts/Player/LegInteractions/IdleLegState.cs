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
        Context.LegActiveState = InputHandler.EBodyActiveState.Down;

        Debug.Log("Enter Idle State");
        Context.LegIKTargetTransform.position = Context._currentLegPos;
        ShakeCamera(0.0f);
    }
    public override void ExitState()
    {
        Context.HipRB.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);

    }
    public override void UpdateState()
    {
       // Context.LegIKTargetTransform.position = Context._currentLegPos;
        Debug.Log("Update Idle State");
        Context.LegIKTargetTransform.position = Context._currentLegPos;
        if(Context.OtherLegActiveState == InputHandler.EBodyActiveState.Down || Context.InputHandler.BodyActiveState == InputHandler.EBodyActiveState.Up)
        {
            Context.HipRB.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.CurrentLegInput == InputHandler.EControllerInput.Up)
        {
            return LegStateMachine.ELegState.Rise;
        }

        bool bodyIsRagdoll = Context.InputHandler.BodyActiveState == InputHandler.EBodyActiveState.Ragdoll;
        bool otherIsRagdoll = Context.OtherLegActiveState == InputHandler.EBodyActiveState.Ragdoll;

        if (bodyIsRagdoll || (otherIsRagdoll && Context.CurrentLegInput != InputHandler.EControllerInput.Reset))
        {
            return LegStateMachine.ELegState.Ragdoll;
        }

        if (!Context.IsGrounded)
        {
            return LegStateMachine.ELegState.Relocate;
        }

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
    public override void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Context.IsGrounded = false;
        }
    }
    private void ShakeCamera(float value)
    {
        Context.CameraShake.AmplitudeGain = value;
        Context.CameraShake.FrequencyGain = value;
    }

}
    