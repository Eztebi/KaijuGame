using Unity.VisualScripting;
using UnityEngine;

public class DebugMoveBodyState : BodyState
{
    private float moveSpeed = 50f;

    public DebugMoveBodyState(BodyStateMachineContext context, BodyStateMachine.EBodyState estate) : base(context, estate)
    {
        BodyStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        foreach(Rigidbody rb in Context.Rigidbodies)
        {
            rb.useGravity = false;
            rb.detectCollisions = false;
        }
        Context.MovingCinemachine.VirtualCamera.Priority = 40;
        Context.MovingCinemachine.HorizontalAxis.Recentering.Enabled = false;
        Context.MovingCinemachine.VerticalAxis.Recentering.Enabled = false;
    }
    public override void ExitState()
    {
        foreach (Rigidbody rb in Context.Rigidbodies)
        {
            rb.useGravity = true;
            rb.detectCollisions = true;
        }
        Context.MovingCinemachine.HorizontalAxis.Recentering.Enabled = true;
        Context.MovingCinemachine.VerticalAxis.Recentering.Enabled = true;
        Context.MovingCinemachine.VirtualCamera.Priority = 20;


    }
    public override void UpdateState()
    {
        DebugMove();
    }
    public override BodyStateMachine.EBodyState GetNextState()
    {
        if(Context.InputHandler.DebugMoveToolInput == InputHandler.EControllerInput.Up)
        {
            return BodyStateMachine.EBodyState.Reset;
        }
        return BodyStateMachine.EBodyState.DebugMode;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void OnCollisionStay(Collision collision) { }
    public override void OnCollisionExit(Collision collision) { }

    void DebugMove()
    {
        Transform cam = Camera.main.transform;
        Vector3 moveDirection = cam.forward * Context.InputHandler.bodyMoveValue.y + cam.right * Context.InputHandler.bodyMoveValue.x;
        if (moveDirection.sqrMagnitude > 1f) moveDirection.Normalize();

        Context.FullBodyPos.transform.position += moveDirection * Time.deltaTime * moveSpeed;  
    }
}

