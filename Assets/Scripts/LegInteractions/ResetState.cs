using UnityEngine;

public class ResetState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 2f;
    private float timeToReset = 1;
    private float timer = 0;
    public ResetState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        timer = 0;
        bodyPos = CheckLegDistance();
        MoveToPosition();
        FeetRestPosition();
        Debug.Log("Enter Reset State");
    }
    public override void ExitState() { }
    public override void UpdateState() {
        //MoveToPosition();
        bodyPos = Vector3.zero;
        Debug.Log("Update Reset State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        //if (timer > timeToReset)
        //{
        return LegStateMachine.ELegState.Idle;
        //}
        ////MoveToPosition();
        //timer += Time.deltaTime;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    public Vector3 CheckLegDistance()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.BodyPosition.position.y, Context._currentLegPos.z);
        float feetHeight = Context.BodyPosition.position.y - Context.LegRestPosition.position.y;

        return pos;
    }
    public override void MoveToPosition()
    {
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);

        Context._currentLegPos = Context.LegRestPosition.position;
        Context.LegIKTargetTransform.position = Context._currentLegPos;
    }
    private void FeetRestPosition()
    {
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.BodyPosition.position.y, Context._currentLegPos.z);
        float feetHeight = Context.BodyPosition.position.y - Context.LegRestPosition.position.y;
        Ray ray = new Ray(pos, Vector3.down * feetHeight);
        if (Physics.Raycast(ray, out RaycastHit hit, feetHeight, Context.GroundMask))
        {
            Context.LegIKTargetTransform.position = hit.point;
            Context._currentLegPos = hit.point;
        }
    }
}
