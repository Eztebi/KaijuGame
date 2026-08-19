using UnityEngine;

public class ResetState : LegInteractionState
{
    private Vector3 targetFootPosGlobal;
    private float moveSpeed = 5f;
    private float timeToReset = 2f;
    private float timer = 0f;

    public ResetState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Context.IsGrounded = true;
        timer = 0;
        targetFootPosGlobal = GetFloorPosition();

        if (Context.CurrentLegPos == Vector3.zero)
        {
            Context.CurrentLegPos = Context.LegIKTargetTransform.position;
        }

        Debug.Log("Enter Reset State - Moviendo SOLO la pierna");
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        // Transición cuando pase el tiempo o cuando el pie ya haya llegado a su meta
        if (timer > timeToReset || Vector3.Distance(Context.CurrentLegPos, targetFootPosGlobal) < .1f)
        {
            return LegStateMachine.ELegState.Idle;
        }
        timer += Time.deltaTime;
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
        targetFootPosGlobal = GetFloorPosition();
        Context.CurrentLegPos = Vector3.Lerp(Context.CurrentLegPos, targetFootPosGlobal, Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Context.CurrentLegPos;
    }

    private Vector3 GetFloorPosition()
    {
        return Context.LegRestPosition.position;
    }
}