using UnityEngine;

public class RotateLeftBodyState : BodyState
{
    private float moveSpeed = 50f;
    private float currentYRotation;

    public RotateLeftBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Context.HipsRB.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        //Context.SetBoneStiffness(0f, 0f);
        //Context.HipsRB.constraints = RigidbodyConstraints.FreezeRotationX;
        //Context.HipsRB.constraints = RigidbodyConstraints.FreezeRotationZ;
        currentYRotation = Context.HipsJoint.targetRotation.eulerAngles.y;
        Debug.Log("Enter RotateLeft State");
    }
    public override void ExitState()
    {
        Context.HipsRB.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);

        //Context.HipsRB.constraints &= ~RigidbodyConstraints.FreezeRotation;
        //Context.SetBoneStiffness(15000f, 500f);
    }
    public override void UpdateState()
    {
        Context.currentLaunchTimer += Time.deltaTime;
        currentYRotation += moveSpeed * Time.deltaTime;
        Quaternion nuevaRotacion = Quaternion.Euler(0f, currentYRotation, 0f);
        Context.HipsJoint.targetRotation = nuevaRotacion;
        Context.FallingCinemachine.HorizontalAxis.Value = Context.MovingCinemachine.HorizontalAxis.Value;
        Context.FallingCinemachine.VerticalAxis.Value = Context.MovingCinemachine.VerticalAxis.Value;
        Context.MovingCinemachine.HorizontalAxis.Value = Context.MovingCinemachine.HorizontalAxis.Center;
        Context.MovingCinemachine.VerticalAxis.Value = Context.MovingCinemachine.VerticalAxis.Center;
        Debug.Log("Update RotateLeft State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if (Context.InputHandler.RightLegActiveState == InputHandler.EBodyActiveState.Ragdoll || Context.InputHandler.LeftLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return BodyStateMachine.EBodyState.Ragdoll;
        }
        if (Context.InputHandler.LeftRotateInput == InputHandler.EControllerInput.Up)
        {
            return BodyStateMachine.EBodyState.Idle;
        }
        if (Context.InputHandler.RightRotateInput == InputHandler.EControllerInput.Down)
        {
            return BodyStateMachine.EBodyState.RotatingRight;
        }
        if (Context.InputHandler.LaunchInput == InputHandler.EControllerInput.Down && Context.TimerToLaunch <= Context.currentLaunchTimer)
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

