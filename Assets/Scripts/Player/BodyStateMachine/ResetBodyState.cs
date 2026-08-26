using UnityEngine;
public class ResetBodyState : BodyState
{
    private Vector3 targetFootPosGlobal;
    private float moveSpeed = 5f;
    private float timeToReset = 2f;
    private float timer = 0f;

    public ResetBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {

    }

    public override void EnterState()
    {
        Context.SetBodyToReset();
        //Context.SetBoneStiffness(15000f, 50f);
        Debug.Log("Enter Body Reset State ");
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        
    }

    public override BodyStateMachine.EBodyState GetNextState()
    {
        return BodyStateMachine.EBodyState.Idle;
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

}