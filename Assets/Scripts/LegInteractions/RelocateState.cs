using UnityEngine;

public class RelocateState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 4f;

    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Enter Relocate State");
    }

    public override void ExitState()
    {
        bodyPos = Vector3.zero;
        legPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        legPos = CheckLegDistance();
        bodyPos = CheckBodyDistance();
        MoveToPosition();
    }

    public override LegStateMachine.ELegState GetNextState()
    {
        float distanceToLeg = Vector3.Distance(Context._currentLegPos, legPos);
        float distanceToBody = Vector3.Distance(Context.FullBodyPos.position, bodyPos);

        // Aumentamos ligeramente el margen de tolerancia a 0.1f para que no se quede bloqueado intentando llegar al milímetro
        if (distanceToLeg < 0.1f && distanceToBody < 0.1f)
        {
            Context.IsGrounded = true;
            return LegStateMachine.ELegState.Touch;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("Choco con suelo");
            Context.IsGrounded = true;
        }
    }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    private float footYOffset = 100f; // Ajusta esto si el pie queda muy arriba o atraviesa el piso

    public Vector3 CheckLegDistance()
    {
        Vector3 rayOrigin = new Vector3(Context.LegIKTargetTransform.position.x, Context.FullBodyPos.position.y + 1f, Context.LegIKTargetTransform.position.z);
        float maxDistance = 10f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxDistance, Context.GroundMask))
        {
            // Dibuja una línea verde en la ventana de Scene para que veas dónde detecta el suelo
            Debug.DrawLine(rayOrigin, hit.point, Color.green);

            // Le sumamos/restamos un pequeño offset para compensar el hueso del tobillo/pie
            return new Vector3(hit.point.x, hit.point.y + footYOffset, hit.point.z);
        }

        // Si no detecta suelo, dibuja línea roja
        Debug.DrawRay(rayOrigin, Vector3.down * maxDistance, Color.red);
        return Context.LegRestPosition.position;
    }

    public Vector3 CheckBodyDistance()
    {
        // 1. Centro horizontal para equilibrar el peso
        Vector3 midPoint = Vector3.Lerp(legPos, Context.OtherLegPosition.position, 0.5f);

        // 2. Calculamos la altura original
        float fixedBodyOffset = Mathf.Abs(Context.FullBodyPos.position.y);

        // 3. LA CORRECCIÓN: Le restamos a la altura para obligar al personaje a "agacharse" un poco.
        // Prueba con -0.2f. Si sigue en el aire, súbelo a -0.3f o -0.4f hasta que pise bien.
        float dropOffset = 0.2f;

        float targetY = (Context.OtherLegPosition.position.y + fixedBodyOffset) - dropOffset;

        return new Vector3(midPoint.x, targetY, midPoint.z);
    }
}