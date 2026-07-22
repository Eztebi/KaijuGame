using UnityEngine;

public class MovingState : LegInteractionState
{
    private Vector3 _calculatedPos;
    private float speed = 3;
    public MovingState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        if (Context.LegIKTargetTransform != null)
        {
            _calculatedPos = Context.LegIKTargetTransform.position;
        }
        Debug.Log("Enter Moving State");
    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Moving State");
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        return CheckToReturnToReset();
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) {
        
    }
    public override void MoveToPosition() {
        _calculatedPos.x -= Time.deltaTime * speed * Context.CurrentLegMoveValue.x;
        _calculatedPos.z -= Time.deltaTime * speed * Context.CurrentLegMoveValue.y;
        Context.LegIKTargetTransform.position = _calculatedPos;

    }
}
