using UnityEngine;

public abstract class BodyState : BaseState<BodyStateMachine.EBodyState>
{
    protected BodyStateMachineContext Context;

    public BodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState stateKey) : base(stateKey)
    {
        Context = context;
    }

    public virtual void MoveToPosition() { }

}
