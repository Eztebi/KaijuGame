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
    public Vector2 leftLegMoveValue;
    public Vector2 rightLegMoveValue;
    InputAction leftLegUpAction;
    InputAction rightLegUpAction;
    InputAction leftLegMoveAction;
    InputAction rightLegMoveAction;
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
        leftLegMoveAction = InputSystem.actions.FindAction("LeftLegMove");
        rightLegMoveAction = InputSystem.actions.FindAction("RightLegMove");
        resetButtonAction = InputSystem.actions.FindAction("Restart");

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
            leftLegMoveValue = leftLegMoveAction.ReadValue<Vector2>();
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
            rightLegMoveValue = rightLegMoveAction.ReadValue<Vector2>();
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

        if (rightLegInput != ELegInput.Up)
        {
            rightLegMoveValue = rightLegMoveAction.ReadValue<Vector2>();
        }
    }
}
