using UnityEngine;
using static InputHandler;

public class RiseState : LegInteractionState
{
    private Vector3 targetBodyPos;
    private float moveSpeed = 3f;

    public RiseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Context.SetLegToRise();
        Context.LegActiveState = InputHandler.ELegActiveState.Up;
        Context.IsGrounded = false;

        // Colocamos la meta inicial donde esté la pierna actualmente
        Context.CurrentLegPos = Context.LegIKTargetTransform.position;
        Debug.Log("Enter Rise State");
    }

    public override void ExitState()
    {
        targetBodyPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        float standingHeightOffset = Mathf.Abs(Context.feetRestPosition.localPosition.y);
        float targetY = Context.OtherLegPosition.position.y + standingHeightOffset;

        targetBodyPos = new Vector3(Context.OtherLegPosition.position.x, targetY, Context.OtherLegPosition.position.z);

        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.OtherLegActiveState == InputHandler.ELegActiveState.Up || Context.OtherLegActiveState == InputHandler.ELegActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
        }

        float distanceToLeg = Vector3.Distance(Context.LegIKTargetTransform.position, Context.LegUpPosition.position);
        if (distanceToLeg < 0.1f)
            return LegStateMachine.ELegState.Move;

        if (Context.CurrentLegInput == InputHandler.ELegInput.Down)
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
        Context.CurrentLegPos= Vector3.Lerp(Context.CurrentLegPos,Context.LegUpPosition.position,Time.deltaTime * moveSpeed);

        Context.LegIKTargetTransform.position = Context.CurrentLegPos;
    }
}
