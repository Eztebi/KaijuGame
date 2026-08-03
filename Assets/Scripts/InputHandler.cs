using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    public enum ELegPos{
        Down,
        Up,
    }
    private ELegPos leftLegState;
    private ELegPos rightLegState;
    public Vector2 leftLegMoveValue;
    public Vector2 rightLegMoveValue;
    InputAction leftLegUpAction;
    InputAction rightLegUpAction;
    InputAction leftLegMoveAction;
    InputAction rightLegMoveAction;
    InputAction leftLegReleaseAction;
    InputAction rightLegReleaseAction;
    public Animator animator;
    public ELegPos LeftLegState => leftLegState;
    public ELegPos RightLegState => rightLegState; 
    private void Start()
    {
        leftLegUpAction = InputSystem.actions.FindAction("LeftLegUp");
        rightLegUpAction = InputSystem.actions.FindAction("RightLegUp");
        leftLegMoveAction = InputSystem.actions.FindAction("LeftLegMove");
        rightLegMoveAction = InputSystem.actions.FindAction("RightLegMove");


        leftLegState = ELegPos.Down;
        rightLegState = ELegPos.Down;
    }
    
    private void OnEnable()
    {
        
    }
    private void Update()
    {
        if (leftLegUpAction.IsPressed()) 
        {
            leftLegState = ELegPos.Up;
            leftLegMoveValue = leftLegMoveAction.ReadValue<Vector2>();
            animator.enabled = true;

            //Debug.Log("Left Leg Up");
            //if (leftLegMoveValue != Vector2.zero) Debug.Log("Left Leg Moving");

        }
        else
        {
            leftLegState = ELegPos.Down;
            animator.enabled = false;
            //Debug.Log("Left Leg Release");
        }
        if(rightLegUpAction.IsPressed())
        {
            rightLegState = ELegPos.Up;
            rightLegMoveValue = rightLegMoveAction.ReadValue<Vector2>();
            animator.enabled = true;

            //Debug.Log("Right Leg Up");
            //if(rightLegMoveValue != Vector2.zero) Debug.Log("Right Leg Moving");  
        }
        else
        {
            rightLegState = ELegPos.Down;
            animator.enabled = false;

            //Debug.Log("Right Leg Realese");
        }
    }
}
