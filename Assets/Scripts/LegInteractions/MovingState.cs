using UnityEngine;
using static InputHandler;

public class MovingState : LegInteractionState
{
    private Vector3 _calculatedPos;
    private float speed = 3;
    private float maxLegDistance = .8f;
    public MovingState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    //public override void AwakeState() { }
    public override void EnterState() {
        Context.LegActiveState = ELegActiveState.Up;

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
        if (Context.OtherLegActiveState == InputHandler.ELegActiveState.Up || Context.OtherLegActiveState == InputHandler.ELegActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
        }

        if (Context.CurrentLegInput == InputHandler.ELegInput.Down)
        {
            return LegStateMachine.ELegState.Release;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) {
        
    }
    public override void MoveToPosition() {
        _calculatedPos.x -= Time.deltaTime * speed * Context.CurrentLegMoveValue.x;
        _calculatedPos.z -= Time.deltaTime * speed * Context.CurrentLegMoveValue.y;

        Vector3 _anchorPoint = new Vector3(Context.LegRestPosition.position.x, _calculatedPos.y, Context.LegRestPosition.position.z);

        Vector3 offset = _calculatedPos - _anchorPoint;
        offset = Vector3.ClampMagnitude(offset, maxLegDistance);

        _calculatedPos = _anchorPoint + offset;


        Context.LegIKTargetTransform.position = _calculatedPos;
        Context._currentLegPos = _calculatedPos;
        

    }
}
