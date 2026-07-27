using UnityEngine;

public class ReleaseState : LegInteractionState
{
    public ReleaseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Release State");

    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Release State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        return CheckToReturnToReset();
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
        float feetHeight = Context.LegUpPosition.position.y - Context.LegRestPosition.position.y;
        Ray ray = new Ray(Context.LegIKTargetTransform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, feetHeight, LayerMask.GetMask("Floor")))
        {
            Context.LegIKTargetTransform.position = hit.point;
            Context._currentLegPos = hit.point;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Context.LegIKTargetTransform.position, Vector3.down);
    }
}
