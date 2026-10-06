using UnityEngine;

public class RotateRightBodyState : BodyState
{
    private float moveSpeed = 50f;
    private float currentYRotation;

    public RotateRightBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Context.HipsRB.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        //Context.HipsRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //Context.SetBoneStiffness(0f, 0f);
        currentYRotation = Context.HipsJoint.targetRotation.eulerAngles.y;
        Debug.Log("Enter RotateRight State");
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
        currentYRotation -= moveSpeed * Time.deltaTime;
        Quaternion nuevaRotacion = Quaternion.Euler(0f, currentYRotation, 0f);
        Context.HipsJoint.targetRotation = nuevaRotacion;

        Debug.Log("Update RotateRight State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if (Context.InputHandler.RightLegActiveState == InputHandler.EBodyActiveState.Ragdoll || Context.InputHandler.LeftLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return BodyStateMachine.EBodyState.Ragdoll;
        }
        if (Context.InputHandler.RightRotateInput == InputHandler.EControllerInput.Up)
        {
            return BodyStateMachine.EBodyState.Idle;
        }
        if(Context.InputHandler.LeftRotateInput == InputHandler.EControllerInput.Down)
        {
            return BodyStateMachine.EBodyState.RotatingLeft;
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

