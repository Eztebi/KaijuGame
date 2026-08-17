using UnityEngine;

public abstract class BodyState : BaseState<BodyStateMachine.EBodyState>
{
    protected BodyStateMachineContext Context;
    private float speed = 1f;

    public BodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState stateKey) : base(stateKey)
    {
        Context = context;
    }

    public virtual void MoveToPosition() { }
    public virtual void CameraAndHipPosMovement()
    {
        Context.OrbitalCamera.HorizontalAxis.Value +=  Time.deltaTime * speed * Context.Handler.cameraMoveValue.x;
        Context.OrbitalCamera.VerticalAxis.Value += Time.deltaTime * speed * Context.Handler.cameraMoveValue.y;
    }
}
