using UnityEngine;

public class IdleBodyState : BodyState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 2f;
    public IdleBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Context.SetBodyToIdle();
        Debug.Log("Enter Idle State");
    }
    public override void ExitState()
    {
     
    }
    public override void UpdateState()
    {
        CameraAndHipPosMovement();
        //Context.HipsJoint.targetPosition = new Vector3(0, -alturaDeseada, 0);
        Debug.Log("Update Idle State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if(Context.Handler.RightLegActiveState == InputHandler.ELegActiveState.Ragdoll || Context.Handler.LeftLegActiveState == InputHandler.ELegActiveState.Ragdoll)
        {
            return BodyStateMachine.EBodyState.Ragdoll;
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