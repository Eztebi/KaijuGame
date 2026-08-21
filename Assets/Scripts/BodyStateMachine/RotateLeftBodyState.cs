using UnityEngine;

public class RotateLeftBodyState : BodyState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 50f;
    private float currentYRotation;

    public RotateLeftBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        //Context.SetBoneStiffness(0f, 0f);
        currentYRotation = Context.HipsJoint.targetRotation.eulerAngles.y;
        Debug.Log("Enter RotateLeft State");
    }
    public override void ExitState()
    {
        //Context.SetBoneStiffness(15000f, 500f);
    }
    public override void UpdateState()
    {
        currentYRotation += moveSpeed * Time.deltaTime;
        Quaternion nuevaRotacion = Quaternion.Euler(0f, currentYRotation, 0f);
        Context.HipsJoint.targetRotation = nuevaRotacion;

        Debug.Log("Update RotateLeft State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if (Context.Handler.RightLegActiveState == InputHandler.EBodyActiveState.Ragdoll || Context.Handler.LeftLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return BodyStateMachine.EBodyState.Ragdoll;
        }
        if (Context.Handler.LeftRotateInput == InputHandler.EControllerInput.Up)
        {
            return BodyStateMachine.EBodyState.Idle;
        }
        if (Context.Handler.RightRotateInput == InputHandler.EControllerInput.Down)
        {
            return BodyStateMachine.EBodyState.RotatingRight;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}

