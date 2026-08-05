using UnityEngine;

public class TouchState : LegInteractionState
{
    public TouchState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Debug.Log("Enter Touch State");
    }
    public override void ExitState() { }
    public override void UpdateState() {
        Debug.Log("Update Touch State");
        MoveToPosition();
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        //return CheckToReturnToReset();
        return LegStateMachine.ELegState.Reset;
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
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.BodyPosition.position.y, Context._currentLegPos.z);
        float feetHeight = Context.BodyPosition.position.y - Context.LegRestPosition.position.y;
        Ray ray = new Ray(pos, Vector3.down * feetHeight);
        if (Physics.Raycast(ray, out RaycastHit hit, feetHeight, LayerMask.GetMask("Floor")))
        {
            Context.LegIKTargetTransform.position = hit.point;
            Context._currentLegPos = hit.point;
            Debug.Log("En suelo");
        }
    }
}
