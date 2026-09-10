using UnityEngine;

public class ResetState : LegInteractionState
{
    private Vector3 targetFootPosGlobal;
    private float moveSpeed = 5f;
    private float timer;

    public ResetState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {

        timer = 0;
        targetFootPosGlobal = GetFloorPosition();
        if (Context._currentLegPos == Vector3.zero)
        {
            Context._currentLegPos = Context.LegIKTargetTransform.position;
        }

        Debug.Log("Enter Reset State");
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        if ( Vector3.Distance(Context._currentLegPos, targetFootPosGlobal) < 0.05f)
        {
            return LegStateMachine.ELegState.Idle;
        }
        if(timer >= 1f)
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

    public override void MoveToPosition()
    {
        targetFootPosGlobal = GetFloorPosition();
        Context._currentLegPos = Vector3.MoveTowards(Context._currentLegPos, targetFootPosGlobal,Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Context._currentLegPos;
    }

    private Vector3 GetFloorPosition()
    {
        return Context.LegRestPosition.position;
    }
}