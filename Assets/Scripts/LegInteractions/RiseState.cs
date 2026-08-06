using UnityEngine;
using static InputHandler;

public class RiseState : LegInteractionState
{
    private Vector3 targetBodyPos;
    private Vector3 targetLegPos;
    private float moveSpeed = 3f;
    public RiseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context,estate) {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState()
    {
        Debug.Log("Enter Rise State");
        Context.IsGrounded = false;
        float maxBodyY = Context.OtherLegPosition.position.y + Context.feetRestPosition.position.y;
        targetBodyPos = new Vector3(Context.OtherLegPosition.position.x,maxBodyY,Context.OtherLegPosition.position.z);

    }
    public override void ExitState() {
        targetBodyPos = Vector3.zero;
        targetLegPos = Vector3.zero;
    }
    public override void UpdateState() {
        targetLegPos = new Vector3(Context._currentLegPos.x, Context.LegUpPosition.position.y, Context._currentLegPos.z);

        MoveToPosition();

        Debug.Log("Update Rise State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        float distanceToLeg = Vector3.Distance(Context.LegIKTargetTransform.position, targetLegPos);
        float distanceToBody = Vector3.Distance(Context.FullBodyPos.position, targetBodyPos);

        if (distanceToLeg < .1f && distanceToBody < .1f)
            return LegStateMachine.ELegState.Move;

        if (Context.CurrentLegState == ELegInput.Down)
            return LegStateMachine.ELegState.Release;

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
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, targetBodyPos, Time.deltaTime * moveSpeed);

        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, targetLegPos, Time.deltaTime * moveSpeed);

        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
