using UnityEngine;
using static InputHandler;

public class MovingState : LegInteractionState
{
    private Vector3 _calculatedPos;

    public MovingState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Context.LegActiveState = EBodyActiveState.Up;

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
        if (Context.OtherLegActiveState == InputHandler.EBodyActiveState.Up || Context.OtherLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
        }

        if (Context.CurrentLegInput == InputHandler.EControllerInput.Down)
        {
            return LegStateMachine.ELegState.Release;
        }
        if (CheckLegDistanceToRagdoll())
        {
            Context.LegActiveState = EBodyActiveState.Ragdoll;
            return LegStateMachine.ELegState.Ragdoll;
        }
        if (Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Down) return LegStateMachine.ELegState.DebugMode;

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) {
        
    }
    public override void MoveToPosition()
    {
        Vector3 desiredPos = _calculatedPos;
        desiredPos.x -= Time.deltaTime * Context.MovingSpeed * Context.CurrentLegMoveValue.x;
        desiredPos.z -= Time.deltaTime * Context.MovingSpeed * Context.CurrentLegMoveValue.y;

        Vector3 _anchorPoint = new Vector3(Context.LegRestPosition.position.x, desiredPos.y, Context.LegRestPosition.position.z);
        Vector3 offset = desiredPos - _anchorPoint;

        if (offset.magnitude > Context.MaxLegMovingDistance)
        {
            Vector3 clampedOffset = Vector3.ClampMagnitude(offset, Context.MaxLegMovingDistance);

            Vector3 movebod = offset - clampedOffset;
            if (Context.FullBodyPos != null)
            {
                Context.FullBodyPos.position += movebod;
            }
            _calculatedPos = _anchorPoint + clampedOffset;
        }
        else
        {
            _calculatedPos = desiredPos;
        }
        Context.LegIKTargetTransform.position = _calculatedPos;
        Context._currentLegPos = _calculatedPos;
    }
    private bool CheckLegDistanceToRagdoll()
    {
        float dist = Vector3.Distance(Context.RagdollLegPos.position, Context.OtherRagdollLegPos.position);
        if (dist > Context.RagdollDistance)
        {
            return true;
        }
        return false;
    }
}
