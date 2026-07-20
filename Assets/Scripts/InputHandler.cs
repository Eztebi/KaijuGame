using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    public enum LegState{
        Down,
        Up,
    }
    private LegState leftLegState;
    private LegState rightLegState;
    public Vector2 leftLegMoveValue;
    public Vector2 rightLegMoveValue;
    InputAction leftLegUpAction;
    InputAction rightLegUpAction;
    InputAction leftLegMoveAction;
    InputAction rightLegMoveAction;
    InputAction leftLegReleaseAction;
    InputAction rightLegReleaseAction;

    public LegState LeftLegState => leftLegState;
    public LegState RightLegState => rightLegState; 
    private void Start()
    {
        leftLegUpAction = InputSystem.actions.FindAction("LeftLegUp");
        rightLegUpAction = InputSystem.actions.FindAction("RightLegUp");
        leftLegMoveAction = InputSystem.actions.FindAction("LeftLegMove");
        rightLegMoveAction = InputSystem.actions.FindAction("RightLegMove");
        leftLegReleaseAction = InputSystem.actions.FindAction("LeftLegRelease");
        rightLegReleaseAction = InputSystem.actions.FindAction("RightLegRelease");

        leftLegState = LegState.Down;
        rightLegState = LegState.Down;
    }
    
    private void OnEnable()
    {
        
    }
    private void Update()
    {
        if (leftLegUpAction.IsPressed()) 
        {
            leftLegState = LegState.Up;
            leftLegMoveValue = leftLegMoveAction.ReadValue<Vector2>();
            Debug.Log("Left Leg Up");
            if (leftLegMoveValue != Vector2.zero) Debug.Log("Left Leg Moving");

        }
        else
        {
            leftLegState = LegState.Down;
            Debug.Log("Left Leg Release");
        }
        if(rightLegUpAction.IsPressed())
        {
            rightLegState = LegState.Up;
            rightLegMoveValue = rightLegMoveAction.ReadValue<Vector2>();
            Debug.Log("Right Leg Up");
            if(rightLegMoveValue != Vector2.zero) Debug.Log("Right Leg Moving");  
        }
        else
        {
            rightLegState = LegState.Down;
            Debug.Log("Right Leg Realese");
        }
    }
}
