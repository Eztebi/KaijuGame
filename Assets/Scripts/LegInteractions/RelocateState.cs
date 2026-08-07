using UnityEngine;

public class RelocateState : LegInteractionState
{
    private Vector3 bodyPos;
    private Vector3 legPos;
    private float moveSpeed = 4f;
    private bool wasFullyRaised;

    private float footYOffset = 0.1f; // Ajusta para que la suela toque el piso y no atraviese
    private float bodyHeightTweak = -0.1f;
    public RelocateState(LegInteractionContext context, LegStateMachine.ELegState estate) : base(context, estate) { }

    public override void EnterState()
    {
        Debug.Log("Enter Relocate State");
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
        if (Mathf.Abs(Context.OtherLegPosition.position.y - Context.LegUpPosition.position.y) < 0.1f)
        {
            Debug.Log("Ambas piernas en el aire -> Ragdoll");
            return LegStateMachine.ELegState.Ragdoll;
        }

        // REGLA 2: Si toca suelo (las distancias llegan a su meta) -> TOUCH
        float distanceToLeg = Vector3.Distance(Context._currentLegPos, legPos);
        float distanceToBody = Vector3.Distance(Context.FullBodyPos.position, bodyPos);

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
    public Vector3 CheckLegDistance()
    {
        Vector3 rayOrigin = new Vector3(Context.LegIKTargetTransform.position.x, Context.FullBodyPos.position.y + 1f, Context.LegIKTargetTransform.position.z);
        float maxDistance = 10f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxDistance, Context.GroundMask))
        {
            // CORRECCIÓN: Le sumamos el footYOffset para que no atraviese el suelo
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
            // CORRECCIÓN: Agregamos bodyHeightTweak por si se estira de más
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
        Context.FullBodyPos.position = Vector3.Lerp(Context.FullBodyPos.position, bodyPos, Time.deltaTime * moveSpeed);
        Context.LegIKTargetTransform.position = Vector3.Lerp(Context.LegIKTargetTransform.position, legPos, Time.deltaTime * (moveSpeed * 1.5f));
        Context._currentLegPos = Context.LegIKTargetTransform.position;
    }
}