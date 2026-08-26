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

    }
    public override void UpdateState()
    {
       // Context.LegIKTargetTransform.position = Context._currentLegPos;
        Debug.Log("Update Idle State");
        Context.LegIKTargetTransform.position = Context._currentLegPos;
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.CurrentLegInput == InputHandler.EControllerInput.Up)
        {
            return LegStateMachine.ELegState.Rise;
        }
        if(Context.OtherLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
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
    private void ShakeCamera(float value)
    {
        Context.CameraShake.AmplitudeGain = value;
        Context.CameraShake.FrequencyGain = value;
    }
}
    