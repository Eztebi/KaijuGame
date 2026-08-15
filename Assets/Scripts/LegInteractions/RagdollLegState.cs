using UnityEngine;
public class RagdollLegState : LegInteractionState
{
    private float _originalHipSpring;

    public RagdollLegState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
    }

    public override void EnterState()
    {
        Context.LegActiveState = InputHandler.ELegActiveState.Ragdoll;
        Debug.Log(Context.LegType + " activando modo Ragdoll (Cae por gravedad)");

        // 1. APAGAR EL IK
        if (Context.CurrentIkConstraint != null)
            Context.CurrentIkConstraint.weight = 0f;

        if (Context.CurrentMultiRotationConstraint != null)
            Context.CurrentMultiRotationConstraint.weight = 0f;

        // 2. AFLOJAR TODOS LOS MÚSCULOS DE LA PIERNA (¡Esto faltaba!)
        // Fuerza 0 y Damper 0 = Muñeco de trapo total.
        Context.SetLegStiffness(0f, 0f);

        // 3. AFLOJAR LA CADERA
        if (Context.HipJoint != null)
        {
            JointDrive drive = Context.HipJoint.slerpDrive;
            _originalHipSpring = drive.positionSpring;
            drive.positionSpring = 0f;
            Context.HipJoint.slerpDrive = drive;
        }

        // --- IMPORTANTE PARA EL CUERPO ---
        // Si tienes un Rigidbody principal en FullBodyPos que antes marcabas como "isKinematic = true"
        // para moverlo por código, AQUÍ es donde debes decirle "isKinematic = false" para que la gravedad actúe.
        // Ejemplo (si descomentas tu Rigidbody en el Context):
        // if (Context.Rb != null) {
        //     Context.Rb.isKinematic = false;
        //     Context.Rb.useGravity = true;
        // }
    }

    public override void ExitState()
    {
        Debug.Log(Context.LegType + " saliendo de Ragdoll, recuperando postura.");

        // 1. ENCENDER IK
        if (Context.CurrentIkConstraint != null) Context.CurrentIkConstraint.weight = 1f;
        if (Context.CurrentMultiRotationConstraint != null) Context.CurrentMultiRotationConstraint.weight = 1f;

        // 2. RECUPERAR FUERZA MUSCULAR
        Context.SetLegStiffness(15000f, 500f); // O los valores por defecto que prefieras

        // 3. RECUPERAR CADERA
        if (Context.HipJoint != null)
        {
            JointDrive drive = Context.HipJoint.slerpDrive;
            drive.positionSpring = _originalHipSpring;
            Context.HipJoint.slerpDrive = drive;
        }

        // Si activaste las físicas del cuerpo en el Enter, aquí debes volver a poner isKinematic = true
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