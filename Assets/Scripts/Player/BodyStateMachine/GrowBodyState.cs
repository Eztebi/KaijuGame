using System.Threading;
using UnityEngine;

public class GrowBodyState : BodyState
{
    private float currentTime;
    float growingTime = 2f;
    bool canExitState = false;

    public GrowBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        canExitState = false;
        currentTime = 0f; 
        Debug.Log("Enter Grow State");
    }
    public override void ExitState()
    {
    }
    public override void UpdateState()
    {
        if (canExitState) return;

        if(currentTime >= growingTime)
        {
            canExitState = true;
            Context.InputHandler.BodyActiveState = InputHandler.EBodyActiveState.Reset;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
        Debug.Log("Update Grow State");
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if (Context.InputHandler.BodyActiveState != InputHandler.EBodyActiveState.Grow) return BodyStateMachine.EBodyState.Reset;

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
}

