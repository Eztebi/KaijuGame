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
        float standingHeightOffset = Mathf.Abs(Context.feetRestPosition.localPosition.y);

        float targetY = Context.OtherLegPosition.position.y + standingHeightOffset;

        targetBodyPos = new Vector3(Context.OtherLegPosition.position.x,targetY,Context.OtherLegPosition.position.z);
    }
    public override void ExitState() {
        targetBodyPos = Vector3.zero;
        targetLegPos = Vector3.zero;
    }
    public override void UpdateState() {
        targetLegPos = Context.LegUpPosition.position;

        MoveToPosition();

        Debug.Log("Update Rise State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        float distanceToLeg = Vector3.Distance(Context.LegIKTargetTransform.position, targetLegPos);
        float distanceToBody = Vector3.Distance(Context.FullBodyPos.position, targetBodyPos);

        if (distanceToLeg < 0.1f && distanceToBody < 0.1f)
            return LegStateMachine.ELegState.Move;

        if (Context.CurrentLegState == InputHandler.ELegInput.Down)
            return LegStateMachine.ELegState.Release;

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    public override void MoveToPosition()
    {
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, targetBodyPos, Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, targetLegPos, Time.deltaTime * moveSpeed);
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
