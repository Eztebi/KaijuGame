using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    public enum EControllerInput
    {
        Down,
        Up,
        Reset
    }
    public enum EBodyActiveState
    {
        Down, Up, Ragdoll, Reset
    }
    private EBodyActiveState bodyActiveState;
    private EBodyActiveState leftLegState;
    private EBodyActiveState rightLegState;
    private EControllerInput leftLegInput;
    private EControllerInput rightLegInput;
    private EControllerInput rightRotateInput;
    private EControllerInput leftRotateInput;
    public Vector2 leftLegMoveValue;
    public Vector2 rightLegMoveValue;
    public Vector2 bodyMoveValue;
    public Vector2 cameraMoveValue;
    InputAction leftLegUpAction;
    InputAction rightLegUpAction;
    InputAction leftLegMoveAction;
    InputAction rightLegMoveAction;
    InputAction resetButtonAction;
    InputAction cameraMoveAction;
    InputAction rotateRightAction;
    InputAction rotateLeftAction;
    InputAction bodyMoveAction;

    [SerializeField] private CinemachineOrbitalFollow cameraOrbital;
    private float cameraSpeed = 1f;

    public CinemachineOrbitalFollow CameraOrbital
    {
        get { return cameraOrbital; }
        set { cameraOrbital = value; }
    }
    public EControllerInput LeftRotateInput
    {
        get { return leftRotateInput; }
        set { leftRotateInput = value; }
    }
    public EControllerInput RightRotateInput
    {
        get { return rightRotateInput; }
        set { rightRotateInput = value; }
    }

    public EControllerInput LeftLegInput
    {
        get { return leftLegInput; }
        set { leftLegInput = value; }
    }

    public EControllerInput RightLegInput
    {
        get
        {
            return rightLegInput;
        }
        set { rightLegInput = value; }
    }
    public EBodyActiveState RightLegActiveState
    {
        get { return rightLegState; }
        set { rightLegState = value; }
    }
    public EBodyActiveState LeftLegActiveState
    {
        get { return leftLegState; }
        set { leftLegState = value; }
    }
    public EBodyActiveState BodyActiveState
    {
        get { return bodyActiveState; }
        set {  bodyActiveState = value; }
    }
    private void Start()
    {
        leftLegUpAction = InputSystem.actions.FindAction("LeftLegUp");
        rightLegUpAction = InputSystem.actions.FindAction("RightLegUp");
        leftLegMoveAction = InputSystem.actions.FindAction("LeftLegMove");
        rightLegMoveAction = InputSystem.actions.FindAction("RightLegMove");
        resetButtonAction = InputSystem.actions.FindAction("Restart");
        cameraMoveAction = InputSystem.actions.FindAction("CameraMove");
        bodyMoveAction = InputSystem.actions.FindAction("BodyMove");
        rotateLeftAction = InputSystem.actions.FindAction("RotateLeft");
        rotateRightAction = InputSystem.actions.FindAction("RotateRight");

        leftLegInput = EControllerInput.Down;
        rightLegInput = EControllerInput.Down;
        rightRotateInput = EControllerInput.Up;
        leftRotateInput = EControllerInput.Up;

    }

    private void OnEnable()
    {

    }
    private void Update()
    {
        //Alzar pie
        if (leftLegUpAction.IsPressed())
        {
            leftLegInput = EControllerInput.Up;
            leftLegMoveValue = leftLegMoveAction.ReadValue<Vector2>();
            //animator.enabled = true;

            Debug.Log("Left Leg Up");
            //if (leftLegMoveValue != Vector2.zero) Debug.Log("Left Leg Moving");

        }
        else
        {
            leftLegInput = EControllerInput.Down;
            leftLegMoveValue = Vector2.zero;
        }
        //Alzar pie
        if (rightLegUpAction.IsPressed())
        {
            rightLegInput = EControllerInput.Up;
            rightLegMoveValue = rightLegMoveAction.ReadValue<Vector2>();

            Debug.Log("Right Leg Up");
        }
        else
        {
            rightLegInput = EControllerInput.Down;
            rightLegMoveValue = Vector2.zero;
        }

        //Pararse
        if (resetButtonAction.IsPressed())
        {
            if (LeftLegActiveState == EBodyActiveState.Ragdoll || RightLegActiveState == EBodyActiveState.Ragdoll)
            {

                    rightLegInput = EControllerInput.Reset;
                    leftLegInput = EControllerInput.Reset;
                
            }
        }
        //ROtacion de cuerpo a derecha
        if (rotateRightAction.IsPressed() && (RightLegActiveState != EBodyActiveState.Ragdoll || LeftLegActiveState != EBodyActiveState.Ragdoll)) {
            rightRotateInput = EControllerInput.Down;
        }
        else rightRotateInput = EControllerInput.Up;

        //Rotacion de cuerpo a izquierda
        if (rotateLeftAction.IsPressed() && (RightLegActiveState != EBodyActiveState.Ragdoll || LeftLegActiveState != EBodyActiveState.Ragdoll))
        {
            leftRotateInput = EControllerInput.Down;
        }
        else leftRotateInput = EControllerInput.Up;
        
        //Movimiento de camara
        if (rightLegInput != EControllerInput.Up)
        {
            cameraMoveValue = cameraMoveAction.ReadValue<Vector2>();
            cameraOrbital.VerticalAxis.Value += cameraMoveValue.y * Time.deltaTime * cameraSpeed;
            cameraOrbital.HorizontalAxis.Value += cameraMoveValue.x * Time.deltaTime * cameraSpeed;
        }
        else cameraMoveValue = Vector2.zero;

        //Movimiento de cuerpo
        if(leftLegInput != EControllerInput.Up || LeftLegActiveState != EBodyActiveState.Ragdoll)
        {
            bodyMoveValue = bodyMoveAction.ReadValue<Vector2>();
        }
    }
}
