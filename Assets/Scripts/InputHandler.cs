using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    public enum ELegInput{
        Down,
        Up,
        Ragdoll
    }
    private ELegInput leftLegState;
    private ELegInput rightLegState;
    public Vector2 leftLegMoveValue;
    public Vector2 rightLegMoveValue;
    InputAction leftLegUpAction;
    InputAction rightLegUpAction;
    InputAction leftLegMoveAction;
    InputAction rightLegMoveAction;
    InputAction leftLegReleaseAction;
    InputAction rightLegReleaseAction;
    public Animator animator;
    public ELegInput LeftLegState => leftLegState;
    public ELegInput RightLegState => rightLegState; 
    private void Start()
    {
        leftLegUpAction = InputSystem.actions.FindAction("LeftLegUp");
        rightLegUpAction = InputSystem.actions.FindAction("RightLegUp");
        leftLegMoveAction = InputSystem.actions.FindAction("LeftLegMove");
        rightLegMoveAction = InputSystem.actions.FindAction("RightLegMove");


        leftLegState = ELegInput.Down;
        rightLegState = ELegInput.Down;
    }
    
    private void OnEnable()
    {
        
    }
    private void Update()
    {
        if (leftLegUpAction.IsPressed()) 
        {
            leftLegState = ELegInput.Up;
            leftLegMoveValue = leftLegMoveAction.ReadValue<Vector2>();
            //animator.enabled = true;

            Debug.Log("Left Leg Up");
            //if (leftLegMoveValue != Vector2.zero) Debug.Log("Left Leg Moving");

        }
        else
        {
            leftLegState = ELegInput.Down;
            //animator.enabled = false;
            //Debug.Log("Left Leg Release");
        }
        if(rightLegUpAction.IsPressed())
        {
            rightLegState = ELegInput.Up;
            rightLegMoveValue = rightLegMoveAction.ReadValue<Vector2>();
           // animator.enabled = true;

            Debug.Log("Right Leg Up");
            //if(rightLegMoveValue != Vector2.zero) Debug.Log("Right Leg Moving");  
        }
        else
        {
            rightLegState = ELegInput.Down;
            //animator.enabled = false;

            //Debug.Log("Right Leg Realese");
        }
    }
}
