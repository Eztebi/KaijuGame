using UnityEngine;

public class RagdollLegState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 4f;

    public RagdollLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState()
    {
        
    }
    public override void ExitState()
    {
       
    }
    public override void UpdateState()
    {
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        //return CheckToReturnToReset();
        return LegStateMachine.ELegState.Idle;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision)
    {
        
    }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
   

    public override void MoveToPosition()
    {
       
    }
}

