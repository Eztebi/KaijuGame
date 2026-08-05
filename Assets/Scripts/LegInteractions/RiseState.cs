using UnityEngine;
using static InputHandler;

public class RiseState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 3f;
    public RiseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context,estate) {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Rise State");
        legPos = CheckLegDistance();
        bodyPos = CheckBodyDistance();
        Context.IsGrounded = false;
    }
    public override void ExitState() {
        bodyPos = Vector3.zero;
        legPos = Vector3.zero;
    }
    public override void UpdateState() {
        MoveToPosition();

        Debug.Log("Update Rise State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {

        float distanceToLegTarget = Vector3.Distance(Context._currentLegPos, legPos);
        float distanceToBodyTarget = Vector3.Distance(Context.FullBodyPos.position, bodyPos);

        if (distanceToLegTarget < .1 && distanceToBodyTarget < .1f)
            return LegStateMachine.ELegState.Move;

        if (Context.CurrentLegState == ELegInput.Down) return LegStateMachine.ELegState.Release;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    public Vector3 CheckBodyDistance()
    {
        float targetY = Context.LegUpPosition.position.y + Context.feetRestPosition.position.y;
        Vector3 midPoint = Vector3.Lerp(Context._currentLegPos, Context.OtherLegPosition.position, 0.3f);

        return new Vector3(midPoint.x, targetY, midPoint.z);
    }

    public Vector3 CheckLegDistance()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.LegUpPosition.position.y, Context._currentLegPos.z);
        return pos;
    }

    public override void MoveToPosition()
    {
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, legPos, Time.deltaTime * moveSpeed);

        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
