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
        if (CheckHasGround()) return LegStateMachine.ELegState.Relocate;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

    public bool CheckHasGround()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.BodyPosition.position.y, Context._currentLegPos.z);
        float feetHeight = Context.BodyPosition.position.y - Context.LegRestPosition.position.y;
        Ray ray = new Ray(pos, Vector3.down * feetHeight);
        if (Physics.Raycast(ray, out RaycastHit hit, feetHeight, Context.GroundMask))
        {
            Debug.Log("hay suelo abajo");
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Context.LegIKTargetTransform.position, Vector3.down);
    }
}
