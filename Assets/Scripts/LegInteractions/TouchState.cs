using UnityEngine;

public class TouchState : LegInteractionState
{
    private float moveSpeed = 6f;
    private Vector3 targetGroundPos;
    public TouchState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Context.IsGrounded = true;
        targetGroundPos = CheckGroundPoint();
        Debug.Log("Enter Touch State");
    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Touch State");
        targetGroundPos = CheckGroundPoint();
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Vector3.Distance(Context.LegIKTargetTransform.position, targetGroundPos) < .1f)
        {
            return LegStateMachine.ELegState.Idle;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

    private Vector3 CheckGroundPoint()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.FullBodyPos.position.y + 1f, Context._currentLegPos.z);
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 10f, Context.GroundMask))
        {
            return hit.point;
        }
        return Context._currentLegPos;
    }

    public override void MoveToPosition()
    {
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, targetGroundPos, Time.deltaTime * moveSpeed);
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
