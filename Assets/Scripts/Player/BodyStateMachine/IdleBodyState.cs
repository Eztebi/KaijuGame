using UnityEngine;

public class IdleBodyState : BodyState
{
    private Transform tempTarget;

    public IdleBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Context.SetBodyToIdle();
        Context.FallingCinemachine.HorizontalAxis.Value = Context.MovingCinemachine.HorizontalAxis.Center;
        Context.FallingCinemachine.VerticalAxis.Value = Context.MovingCinemachine.VerticalAxis.Center;
        Context.FallingCinemachine.VirtualCamera.Priority = 10;
        Context.MovingCinemachine.VirtualCamera.Priority = 20;
        Debug.Log("Enter Idle State");
    }
    public override void ExitState()
    {
     
    }
    public override void UpdateState()
    {
        Context.currentLaunchTimer += Time.deltaTime;
        //Context.HipsJoint.targetPosition = new Vector3(0, -alturaDeseada, 0);
        Debug.Log("Update Idle State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if(Context.InputHandler.RightLegActiveState == InputHandler.EBodyActiveState.Ragdoll || Context.InputHandler.LeftLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return BodyStateMachine.EBodyState.Ragdoll;
        }
        if(Context.InputHandler.RightRotateInput == InputHandler.EControllerInput.Down)
        {
            return BodyStateMachine.EBodyState.RotatingRight;
        }
        if(Context.InputHandler.LeftRotateInput == InputHandler.EControllerInput.Down)
        {
            return BodyStateMachine.EBodyState.RotatingLeft;
        }
        if(Context.InputHandler.LaunchInput == InputHandler.EControllerInput.Down && Context.TimerToLaunch <= Context.currentLaunchTimer)
        {
            return BodyStateMachine.EBodyState.Launch;
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