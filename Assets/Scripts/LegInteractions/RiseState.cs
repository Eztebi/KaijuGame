using UnityEngine;
using static InputHandler;

public class RiseState : LegInteractionState
{
    private Vector3 targetBodyPos;
    private Vector3 targetLegPos;
    private float moveSpeed = 3f;

    public RiseState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        // Usa la propiedad heredada 'Context' de la clase base
    }

    public override void EnterState()
    {
        Context.LegActiveState = InputHandler.ELegActiveState.Up;
        Debug.Log("Enter Rise State");
        Context.IsGrounded = false;

        // Ajustamos la firmeza muscular para levantar la pierna
        Context.SetLegStiffness(4000f, 80f);
        targetLegPos = Context.LegUpPosition.position;
    }

    public override void ExitState()
    {
        targetBodyPos = Vector3.zero;
        targetLegPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        // 1. Posición objetivo para la pierna que se alza

        // 2. Posición objetivo para el cuerpo (Hips) SOBRE la otra pierna (la apoyada)
        float standingHeightOffset = Mathf.Abs(Context.feetRestPosition.localPosition.y);
        float targetY = Context.OtherLegPosition.position.y + standingHeightOffset;

        // Alineamos X y Z del cuerpo exactamente sobre la pierna de apoyo, manteniendo la altura en Y
        targetBodyPos = new Vector3(Context.OtherLegPosition.position.x,targetY,Context.OtherLegPosition.position.z);

        // 3. Movemos tanto el cuerpo como la pierna
        MoveToPosition();

        Debug.Log("Update Rise State");
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.OtherLegActiveState == InputHandler.ELegActiveState.Up || Context.OtherLegActiveState == InputHandler.ELegActiveState.Ragdoll)
        {
            return LegStateMachine.ELegState.Ragdoll;
        }

        float distanceToLeg = Vector3.Distance(Context.LegIKTargetTransform.position, targetLegPos);
        if (distanceToLeg < 0.1f)
            return LegStateMachine.ELegState.Move;

        if (Context.CurrentLegInput == InputHandler.ELegInput.Down)
            return LegStateMachine.ELegState.Release;

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
        // Desplazamos las caderas/cuerpo hacia el objetivo sobre la pierna de apoyo
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, targetBodyPos, Time.deltaTime * moveSpeed);

        // Subimos el IK del pie que se alza
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, targetLegPos, Time.deltaTime * moveSpeed);
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}
