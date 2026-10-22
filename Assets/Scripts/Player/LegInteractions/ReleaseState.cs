using UnityEngine;
using static InputHandler;

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
    public override void ExitState() {
        //Context.HipJoint.xMotion = ConfigurableJointMotion.Free;
        //Context.HipJoint.zMotion = ConfigurableJointMotion.Free;
        //Context.HipJoint.yMotion = ConfigurableJointMotion.Free;
    }
    public override void UpdateState() {
        Debug.Log("Update Release State");
    }
    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.OtherLegActiveState == EBodyActiveState.Up) return LegStateMachine.ELegState.Ragdoll;
        if (Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Down) return LegStateMachine.ELegState.DebugMode;

        return LegStateMachine.ELegState.Relocate;
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
        Vector3 pos = new Vector3(Context._currentLegPos.x, Context.FullBodyPos.position.y, Context._currentLegPos.z);

        float maxDistance = Mathf.Abs(Context.FullBodyPos.position.y - Context.LegRestPosition.position.y) + 2f;

        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, maxDistance, Context.GroundMask))
        {
            Debug.Log("Hay suelo abajo Relocate");
            return true;
        }

        Debug.Log("No hay suelo abajo Ragdoll");
        return false;
    }
}

