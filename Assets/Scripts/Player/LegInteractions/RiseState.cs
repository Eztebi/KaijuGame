using System.Drawing;
using Unity.Cinemachine;
using UnityEngine;
using static InputHandler;

public class RiseState : LegInteractionState
{
    private Vector3 targetBodyPos;
    private float moveSpeed = 3f;
    JointDrive savedDrive;
    public RiseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {
        savedDrive = Context.HipJoint.slerpDrive;
        savedDrive.positionSpring = Context.HipJoint.slerpDrive.positionSpring;
        savedDrive.positionDamper = Context.HipJoint.slerpDrive.positionDamper;

        //Context.SetJointSlerp(Context.HipJoint, 25000f, 500f);
        Context.SetLegToRise();

        Context.LegActiveState = InputHandler.EBodyActiveState.Up;
        Debug.Log("Enter Rise State");
        Context.IsGrounded = false;

    }

    public override void ExitState()
    {
        //Context.SetJointSlerp(Context.HipJoint, savedDrive.positionSpring, savedDrive.positionDamper);
        targetBodyPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        float standingHeightOffset = Mathf.Abs(Context.LegRestPosition.localPosition.y);
        float targetY = Context.OtherLegPosition.position.y + standingHeightOffset;

        targetBodyPos = new Vector3(Context.OtherLegPosition.position.x - Context.LegRestPosition.localPosition.x, targetY, Context.OtherLegPosition.position.z);

        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.OtherLegActiveState == InputHandler.EBodyActiveState.Up || Context.OtherLegActiveState == InputHandler.EBodyActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
        }

        float distanceToLeg = Vector3.Distance(Context.LegIKTargetTransform.position, Context.LegUpPosition.position);
        if (distanceToLeg < 0.1f)
            return LegStateMachine.ELegState.Move;

        if (Context.CurrentLegInput == InputHandler.EControllerInput.Down)
            return LegStateMachine.ELegState.Release;
        if (Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Down) return LegStateMachine.ELegState.DebugMode;

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
        float speed = moveSpeed * Time.deltaTime;

        Context.FullBodyPos.position = Vector3.MoveTowards(Context.FullBodyPos.position, targetBodyPos, speed);
        Context.LegIKTargetTransform.position = Vector3.MoveTowards(Context.LegIKTargetTransform.position, Context.LegUpPosition.position, speed);

        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
