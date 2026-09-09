using UnityEngine;

public class LaunchBody : BodyState
{
    private float launchForce = 7f;
    Vector3 pushDirection;
    public LaunchBody(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        //Conte.xt.SetBoneStiffness(0f, 0f);
        Context.Handler.BodyActiveState = InputHandler.EBodyActiveState.Ragdoll;
        pushDirection = Context.SpinePos.forward;
        Context.SetBodyToRagdoll();
        Launch();
        Debug.Log("Enter Falling State");
    }
    public override void ExitState()
    {
        //Context.SetBoneStiffness(15000f, 500f);
    }
    public override void UpdateState()
    {
        Debug.Log("Update Falling State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        return BodyStateMachine.EBodyState.Ragdoll;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    private void Launch()
    {
        foreach (Rigidbody rb in Context.Rigidbodies)
        {
            rb.AddForce(pushDirection * launchForce, ForceMode.VelocityChange);
        }
    }
}
