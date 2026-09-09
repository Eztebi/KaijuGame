using UnityEngine;

public class RagdollBodyState : BodyState
{
    public RagdollBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        //Context.SetBoneStiffness(0f, 0f);
        Context.SetBodyToRagdoll();
        Debug.Log("Enter Falling State");
    }
    public override void ExitState()
    {
        Context.SetBoneStiffness(15000f, 500f);
        Context.Handler.BodyActiveState = InputHandler.EBodyActiveState.Reset;
    }
    public override void UpdateState()
    {
        Debug.Log("Update Falling State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if (Context.Handler.RightLegActiveState != InputHandler.EBodyActiveState.Ragdoll || Context.Handler.LeftLegActiveState != InputHandler.EBodyActiveState.Ragdoll || Context.Handler.BodyActiveState != InputHandler.EBodyActiveState.Ragdoll)
        {
            return BodyStateMachine.EBodyState.Reset;
        }
        if(Context.Handler.BodyActiveState == InputHandler.EBodyActiveState.Reset)
        {
            return BodyStateMachine.EBodyState.Reset;
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
