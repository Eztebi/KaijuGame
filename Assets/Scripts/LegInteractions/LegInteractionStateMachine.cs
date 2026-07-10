using UnityEngine;
using UnityEngine.Animations.Rigging;
public class LegInteractionStateMachine : StateManager<LegInteractionStateMachine.ELegInteractionState>
{
    public enum ELegInteractionState
    {
        Relocate,
        Rise,
        Move,
        Release,
        Touch,
        Reset
    }

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _RightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
}
