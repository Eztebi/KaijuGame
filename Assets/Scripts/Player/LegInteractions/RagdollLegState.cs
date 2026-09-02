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
        Context.LegActiveState = InputHandler.EBodyActiveState.Ragdoll;
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

        if (Context.CurrentIkConstraint != null) Context.CurrentIkConstraint.weight = 1f;
        if (Context.CurrentMultiRotationConstraint != null) Context.CurrentMultiRotationConstraint.weight = 1f;

    }

    public override void UpdateState() { }

    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.CurrentLegInput == InputHandler.EControllerInput.Reset)
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
        if (!collision.collider.CompareTag("Player")) ShakeCamera(1.0f);
    }

    public override void OnCollisionStay(Collision collision) {
        if (!collision.collider.CompareTag("Player")) ShakeCamera(1.0f); ShakeCamera(0.0f);
    }
    public override void OnCollisionExit(Collision collision) { }
    private void ShakeCamera(float value)
    {
        Context.CameraShake.AmplitudeGain = value;
        Context.CameraShake.FrequencyGain = value;
    }
    public override void MoveToPosition()
    {
        // Vacío, la física controla la posición ahora.
    }
}