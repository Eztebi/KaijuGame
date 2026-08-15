using UnityEngine;

public abstract class LegInteractionState : BaseState<LegStateMachine.ELegState>
{
    protected LegInteractionContext Context;

    public LegInteractionState(LegInteractionContext context, LegStateMachine.ELegState stateKey): base(stateKey) {
        Context = context;
    }

    public virtual void MoveToPosition() { }
}
