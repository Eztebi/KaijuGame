using UnityEngine;
public class RagdollLegState : LegInteractionState
{
    private float _originalHipSpring;

    public RagdollLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Context.IsGrounded = false;
        Context.LegActiveState = InputHandler.ELegActiveState.Ragdoll;
        Debug.Log(Context.LegType + " activando modo Ragdoll (Cae por gravedad)");

        if (Context.CurrentIkConstraint != null)
            Context.CurrentIkConstraint.weight = 0f;

        if (Context.CurrentMultiRotationConstraint != null)
            Context.CurrentMultiRotationConstraint.weight = 0f;

        Context.SetLegStiffness(0f, 0f);

    }

    public override void ExitState()
    {
        Debug.Log(Context.LegType + " saliendo de Ragdoll, recuperando postura.");

        // 1. ENCENDER IK
        if (Context.CurrentIkConstraint != null) Context.CurrentIkConstraint.weight = 1f;
        if (Context.CurrentMultiRotationConstraint != null) Context.CurrentMultiRotationConstraint.weight = 1f;

    }

    public override void UpdateState() { }

    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.CurrentLegInput == InputHandler.ELegInput.Reset)
        {
            return LegStateMachine.ELegState.Reset;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }

    public override void OnCollisionEnter(Collision collision)
    {
        // Opcional: Aquí podrías reproducir sonidos de golpes fuertes si la velocidad de choque es alta
    }

    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

    public override void MoveToPosition()
    {
        // Vacío, la física controla la posición ahora.
    }
}