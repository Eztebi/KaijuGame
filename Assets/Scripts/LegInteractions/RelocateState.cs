using UnityEngine;

public class RelocateState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 4f;
    private bool wasFullyRaised;

    private float footYOffset = 0.1f;
    private float bodyHeightTweak = -0.1f;

    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate) 
    {
        LegInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Relocate State");
        Context.IsGrounded = false; // Reiniciamos el estado de suelo

        Vector3 legpos = new Vector3(Context._currentLegPos.x, Context.LegUpPosition.position.y, Context._currentLegPos.z);
        float distanceToTop = Vector3.Distance(legpos, Context.LegUpPosition.position);
        wasFullyRaised = distanceToTop < 0.1f;
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
        //if (Mathf.Abs(Context.OtherLegPosition.position.y - Context.LegUpPosition.position.y) < 0.1f)
        //{
        //    Debug.Log("Ambas piernas en el aire -> Ragdoll");
        //    return LegStateMachine.ELegState.Ragdoll;
        //}

        // ¡AQUÍ ESTÁ EL CAMBIO!
        // Si el collider físico tocó el suelo o escalón, pasamos de inmediato a Touch
        if (Context.IsGrounded)
        {
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
            Debug.Log("Choco con suelo o escalon");
            Context.IsGrounded = true;

            // Congelamos la posición actual del IK Target donde ocurrió el choque
            // para que no siga empujando hacia abajo atravesando el piso.
            Context._currentLegPos = Context.LegIKTargetTransform.position;
        }
    }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }
    public Vector3 CheckLegDistance()
    {
        Vector3 rayOrigin = new Vector3(Context.LegIKTargetTransform.position.x, Context.FullBodyPos.position.y + 1f, Context.LegIKTargetTransform.position.z);
        float maxDistance = 10f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxDistance, Context.GroundMask))
        {
            return new Vector3(hit.point.x, hit.point.y + footYOffset, hit.point.z);
        }

        return Context.LegRestPosition.position;
    }

    public Vector3 CheckBodyDistance()
    {
        Vector3 midPoint = Vector3.Lerp(legPos, Context.OtherLegPosition.position, 0.5f);
        float fixedBodyOffset = Mathf.Abs(Context.feetRestPosition.localPosition.y);

        float targetY;

        if (wasFullyRaised)
        {
            targetY = Context.OtherLegPosition.position.y + fixedBodyOffset + bodyHeightTweak;
        }
        else
        {
            targetY = (legPos.y + fixedBodyOffset) - 0.4f;
        }

        return new Vector3(midPoint.x, targetY, midPoint.z);
    }

    public override void MoveToPosition()
    {
        // Si ya tocó suelo, dejamos de mover el IK Target hacia abajo
        if (Context.IsGrounded) return;

        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, legPos, Time.deltaTime * (moveSpeed * 1.5f));
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}