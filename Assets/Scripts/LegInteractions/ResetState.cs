using UnityEngine;

public class ResetState : LegInteractionState
{
    private Vector3 targetFootPosGlobal;
    private float moveSpeed = 5f;
    private float timeToReset = 2f;
    private float timer = 0f;

    public ResetState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
    }

    public override void EnterState()
    {
        timer = 0;
        //Context.SetLegStiffness(15000f, 500f);

        // 1. Meta de los pies (Dónde debe aterrizar)
        targetFootPosGlobal = GetFloorPosition();

        // Prevención del bug 0,0,0
        if (Context._currentLegPos == Vector3.zero)
        {
            Context._currentLegPos = Context.LegIKTargetTransform.position;
        }

        Debug.Log("Enter Reset State - Moviendo SOLO la pierna");
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        // Transición cuando pase el tiempo o cuando el pie ya haya llegado a su meta
        if (timer > timeToReset || Vector3.Distance(Context._currentLegPos, targetFootPosGlobal) < 0.05f)
        {
            return LegStateMachine.ELegState.Idle;
        }
        timer += Time.deltaTime;
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
        // Actualizamos la meta en tiempo real por si el cuerpo (padre) se está moviendo por otra razón
        targetFootPosGlobal = GetFloorPosition();

        // Movemos SOLO la posición global de la pierna hacia su descanso
        Context._currentLegPos = Vector3.Lerp(Context._currentLegPos, targetFootPosGlobal, Time.deltaTime * moveSpeed);

        // Aplicamos la posición al target del IK
        Context.LegIKTargetTransform.position = Context._currentLegPos;
    }

    private Vector3 GetFloorPosition()
    {
        // Si quieres usar el raycast de nuevo, descoméntalo aquí.
        // Por ahora, simplemente devuelve la posición global de descanso.
        return Context.LegRestPosition.position;
    }
}