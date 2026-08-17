using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    public enum ELegInput
    {
        Down,
        Up,
        Reset
    }
    public enum ELegActiveState
    {
        Down, Up, Ragdoll, Reset
    }
    private ELegActiveState leftLegState;
    private ELegActiveState rightLegState;
    private ELegInput leftLegInput;
    private ELegInput rightLegInput;
    public Vector2 legMoveValue;
    public Vector2 cameraMoveValue;
    InputAction leftLegUpAction;
    InputAction rightLegUpAction;
    InputAction legMoveAction;
    InputAction cameraMoveAction;
    InputAction resetButtonAction;
    public Animator animator;
    public ELegInput LeftLegInput
    {
        get { return leftLegInput; }
        set { leftLegInput = value; }
    }

    public ELegInput RightLegInput
    {
        get
        {
            return rightLegInput;
        }
        set { rightLegInput = value; }
    }
    public ELegActiveState RightLegActiveState
    {
        get { return rightLegState; }
        set { rightLegState = value; }
    }
    public ELegActiveState LeftLegActiveState
    {
        get { return leftLegState; }
        set { leftLegState = value; }
    }
    private void Start()
    {
        leftLegUpAction = InputSystem.actions.FindAction("LeftLegUp");
        rightLegUpAction = InputSystem.actions.FindAction("RightLegUp");
        legMoveAction = InputSystem.actions.FindAction("LegMove");
        resetButtonAction = InputSystem.actions.FindAction("Restart");
        cameraMoveAction = InputSystem.actions.FindAction("CameraMove");

        leftLegInput = ELegInput.Down;
        rightLegInput = ELegInput.Down;
    }

    private void OnEnable()
    {

    }
    private void Update()
    {
        if (leftLegUpAction.IsPressed())
        {
            leftLegInput = ELegInput.Up;
            legMoveValue = legMoveAction.ReadValue<Vector2>();
            //animator.enabled = true;

            Debug.Log("Left Leg Up");
            //if (leftLegMoveValue != Vector2.zero) Debug.Log("Left Leg Moving");

        }
        else
        {
            leftLegInput = ELegInput.Down;
            //animator.enabled = false;
            //Debug.Log("Left Leg Release");
        }
        if (rightLegUpAction.IsPressed())
        {
            rightLegInput = ELegInput.Up;
            legMoveValue = legMoveAction.ReadValue<Vector2>();
            // animator.enabled = true;

            Debug.Log("Right Leg Up");
            //if(rightLegMoveValue != Vector2.zero) Debug.Log("Right Leg Moving");  
        }
        else
        {
            rightLegInput = ELegInput.Down;
            //animator.enabled = false;

            //Debug.Log("Right Leg Realese");
        }
        if (resetButtonAction.IsPressed())
        {
            if (LeftLegActiveState == ELegActiveState.Ragdoll || RightLegActiveState == ELegActiveState.Ragdoll)
            {

                    rightLegInput = ELegInput.Reset;
                    leftLegInput = ELegInput.Reset;
                
            }
        }

        cameraMoveValue = cameraMoveAction.ReadValue<Vector2>();    
    }
}
