using UnityEngine;
using static InputHandler;

public class RelocateState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = .2f;

    private float bodyHeightTweak = -0.1f;

    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter Relocate State");
        Context.IsGrounded = false;
    }

    public override void ExitState()
    {
        bodyPos = Vector3.zero;
        legPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        // 1. EL PIE VA DIRECTO A SU META: 
        // Se mueve en X, Y y Z al mismo tiempo. (Le restamos 2f en Y para forzar el choque físico).
        legPos = new Vector3(Context.LegRestPosition.position.x, Context.LegRestPosition.position.y - 2.0f, Context.LegRestPosition.position.z);

        // 2. ¿CUÁNDO BAJA EL CUERPO?
        // Medimos qué tan lejos está el pie de su punto más alto (UpPos)
        float distanciaRecorrida = Vector3.Distance(Context.LegIKTargetTransform.position, Context.LegUpPosition.position);
        float distanciaTotal = Vector3.Distance(Context.LegUpPosition.position, Context.LegRestPosition.position);

        // Si el pie ya recorrió más de la mitad del camino, el cuerpo empieza a bajar.
        // (Puedes cambiar el 0.5f por 0.3f si quieres que el cuerpo baje antes, o 0.8f para que baje casi al final)
        bool bajarCuerpo = distanciaRecorrida > (distanciaTotal * 0.5f);

        // 3. CALCULAR META DEL CUERPO (Siempre centrado en X y Z)
        float midX = (Context.LegIKTargetTransform.position.x + Context.OtherLegPosition.position.x) / 2f;
        float midZ = (Context.LegIKTargetTransform.position.z + Context.OtherLegPosition.position.z) / 2f;

        float fixedBodyOffset = Mathf.Abs(Context.feetRestPosition.localPosition.y);
        float targetY;

        if (!bajarCuerpo)
        {
            // El cuerpo se mantiene arriba apoyado en la pierna firme
            targetY = Context.OtherLegPosition.position.y + fixedBodyOffset + bodyHeightTweak;
        }
        else
        {
            // El cuerpo acompaña la caída del pie
            targetY = (Context.LegIKTargetTransform.position.y + fixedBodyOffset) - 0.4f;

            // Límite para que no se hunda demasiado
            float minHeight = Context.OtherLegPosition.position.y + fixedBodyOffset - 0.6f;
            targetY = Mathf.Max(targetY, minHeight);
        }

        bodyPos = new Vector3(midX, targetY, midZ);

        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        if (Context.OtherLegActiveState == ELegActiveState.Up) return LegStateMachine.ELegState.Ragdoll;

        if (Context.IsGrounded)
        {
            Context.LegActiveState = ELegActiveState.Down;
            return LegStateMachine.ELegState.Touch;
        }

        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }

    public override void OnCollisionEnter(Collision collision)
    {
        // Revisamos contra qué chocó exactamente
        Debug.LogWarning(Context.LegType + " ENTER chocó contra: " + collision.collider.name + " Tag: " + collision.collider.tag);

        if (collision.collider.CompareTag("Ground"))
        {
            DetenerPie();
        }
    }

    // ¡AQUÍ ESTÁ LA MAGIA NUEVA!
    public override void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && !Context.IsGrounded)
        {
            Debug.LogWarning(Context.LegType + " STAY detectó el suelo a medio camino. ¡Deteniéndose!");
            DetenerPie();
        }
    }

    public override void OnCollisionExit(Collision collision) { }

    // Creamos este método pequeñito para no repetir código
    private void DetenerPie()
    {
        Debug.Log(Context.LegType + " tocó el suelo. ¡Deteniéndose!");
        Context.IsGrounded = true;
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
    public override void MoveToPosition()
    {
        // Lerp del cuerpo
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);

        // Lerp del pie (si no ha chocado)
        if (!Context.IsGrounded)
        {
            Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, legPos, Time.deltaTime * (moveSpeed * 1.5f));
            Context._currentLegPos = Context.LegIKTargetTransform.position;
        }
    }
}