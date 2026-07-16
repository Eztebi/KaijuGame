using UnityEngine;

public abstract class LegInteractionState : BaseState<LegInteractionStateMachine.ELegInteractionState>
{
    protected LegInteractionContext Context;

    public LegInteractionState(LegInteractionContext context, LegInteractionStateMachine.ELegInteractionState stateKey): base(stateKey) {
        Context = context;
    }
    
    
}
