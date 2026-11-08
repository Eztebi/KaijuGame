using UnityEngine;

public class RagdollBodyState : BodyState
{
    public RagdollBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Context.FallingCinemachine.HorizontalAxis.Value = Context.MovingCinemachine.HorizontalAxis.Value;
        Context.FallingCinemachine.HorizontalAxis.Center = Context.MovingCinemachine.HorizontalAxis.Center;
        Context.FallingCinemachine.VerticalAxis.Value = Context.MovingCinemachine.VerticalAxis.Value;
        Context.FallingCinemachine.VerticalAxis.Center = Context.MovingCinemachine.VerticalAxis.Center;
        Context.FallingCinemachine.VirtualCamera.Follow = null;
        Context.MovingCinemachine.VirtualCamera.Follow = null;
        Context.FallingCinemachine.VirtualCamera.Priority = 20;
        Context.MovingCinemachine.VirtualCamera.Priority = 10;
        Context.SetBoneStiffness(0f, 0f);
        Context.SetBodyToRagdoll();
        Debug.Log("Enter Falling State");
        Context.InputHandler.BodyActiveState = InputHandler.EBodyActiveState.Ragdoll;
    }
    public override void ExitState()
    {
        Context.SetBoneStiffness(15000f, 500f);
        //Context.FallingCinemachine.VirtualCamera.Follow = tempTarget;
    }
    public override void UpdateState()
    {
        
        Debug.Log("Update Falling State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if (Context.InputHandler.LeftLegInput == InputHandler.EControllerInput.Reset || Context.InputHandler.RightLegInput == InputHandler.EControllerInput.Reset)
        {
            return BodyStateMachine.EBodyState.Reset;
        }

        if (Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Down) return BodyStateMachine.EBodyState.DebugMode;

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}
