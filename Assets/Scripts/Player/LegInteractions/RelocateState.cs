using Unity.VisualScripting;
using UnityEngine;
using static InputHandler;

public class RelocateState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 2f;

    private float bodyHeightTweak = -0.1f;

    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Context.LegActiveState = InputHandler.EBodyActiveState.Down;
        Debug.Log("Enter Relocate State");
    }

    public override void ExitState()
    {
        bodyPos = Vector3.zero;
        legPos = Vector3.zero;
        ShakeCamera(0.0f);
    }

    public override void UpdateState()
    {
        legPos = new Vector3(Context.LegRestPosition.position.x, Context.LegRestPosition.position.y - 2.0f, Context.LegRestPosition.position.z);
        float distanciaRecorrida = Vector3.Distance(Context.LegIKTargetTransform.position, Context.LegUpPosition.position);
        float distanciaTotal = Vector3.Distance(Context.LegUpPosition.position, Context.LegRestPosition.position);
        bool bajarCuerpo = distanciaRecorrida > (distanciaTotal * 0.5f);
        float midX = (Context.LegIKTargetTransform.position.x + Context.OtherLegPosition.position.x) / 2f;
        float midZ = (Context.LegIKTargetTransform.position.z + Context.OtherLegPosition.position.z) / 2f;

        float fixedBodyOffset = Mathf.Abs(Context.LegRestPosition.localPosition.y);
        float targetY;

        if (!bajarCuerpo)
        {
            targetY = Context.OtherLegPosition.position.y + fixedBodyOffset + bodyHeightTweak;
        }
        else
        {
            targetY = (Context.LegIKTargetTransform.position.y + fixedBodyOffset) - 0.4f;

            float minHeight = Context.OtherLegPosition.position.y + fixedBodyOffset - 0.6f;
            targetY = Mathf.Max(targetY, minHeight);
        }

        bodyPos = new Vector3(midX, targetY, midZ);

        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        //if (Context.OtherLegActiveState == EBodyActiveState.Up) return LegStateMachine.ELegState.Ragdoll;

        if (Context.IsGrounded)
        {
            Context.LegActiveState = EBodyActiveState.Down;
            return LegStateMachine.ELegState.Touch;
        }
        else if (CheckLegDistanceToRagdoll())
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

    public override void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Ground"))
        {
            //ShakeCamera(1.0f);
            DetenerPie();
        }
    }
    public override void OnCollisionStay(Collision collision)
    {
    }
    public override void OnCollisionExit(Collision collision) {

    }
    private void ShakeCamera(float value)
    {
        Context.CameraShake.AmplitudeGain = value;
        Context.CameraShake.FrequencyGain = value;
    }
    private void DetenerPie()
    {
        Context.IsGrounded = true;
        Context.LegActiveState = InputHandler.EBodyActiveState.Down;
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
    public override void MoveToPosition()
    {
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);
        if (!Context.IsGrounded)
        {
            Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, legPos, Time.deltaTime * (moveSpeed * 1.5f));
            Context._currentLegPos = Context.LegIKTargetTransform.position;
        }
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