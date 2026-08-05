using UnityEngine;

public class RelocateState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 4f;

    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        legPos = CheckLegDistance();
        bodyPos = CheckBodyDistance();
        Debug.Log("Enter Relocate State");
    }
    public override void ExitState() {
        bodyPos = Vector3.zero;
        legPos = Vector3.zero;
        Debug.Log("Update Relocate State");
    }
    public override void UpdateState()
    {
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        //return CheckToReturnToReset();
        float distanceToTarget = Vector3.Distance(Context._currentLegPos, legPos);
        if (distanceToTarget < .1f)
        {
            Context.IsGrounded = true;
            return LegStateMachine.ELegState.Touch;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("Choco con suelo");
            Context.IsGrounded = true;
        }
    }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    public Vector3 CheckBodyDistance()
    {
        Vector3 midPoint = Vector3.Lerp(Context._currentLegPos, Context.OtherLegPosition.position, 0.3f);

        return new Vector3(midPoint.x, Context.FullBodyPos.position.y, midPoint.z);
    }

    public Vector3 CheckLegDistance()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.BodyPosition.position.y, Context._currentLegPos.z);
        float feetHeight = Context.BodyPosition.position.y - Context.LegRestPosition.position.y;

        Ray ray = new Ray(pos, Vector3.down * feetHeight);

        if (Physics.Raycast(ray, out RaycastHit hit, feetHeight, Context.GroundMask))
        {
            return hit.point;
        }
        return pos;
    }

    public override void MoveToPosition()
    {
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, legPos, Time.deltaTime * moveSpeed);

        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
