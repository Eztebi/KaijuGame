using UnityEngine;

public class ActivateRagdollBodyState : BodyState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 2f;
    public ActivateRagdollBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        Context.SetBodyToRagdoll();
        Debug.Log("Enter Activateragdoll State");
    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
        Debug.Log("Update Activate Ragdoll State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}
