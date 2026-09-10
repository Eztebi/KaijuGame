using Unity.Cinemachine;
using UnityEditor;
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
        Down, Up, Ragdoll, Reset, Launch
    }
    private EBodyActiveState bodyActiveState;
    private EBodyActiveState leftLegState;
    private EBodyActiveState rightLegState;
    private EControllerInput leftLegInput;
    private EControllerInput rightLegInput;
    private EControllerInput rightRotateInput;
    private EControllerInput leftRotateInput;
    private EControllerInput launchInput;
    public Vector2 leftLegMoveValue;
    public Vector2 rightLegMoveValue;
    public Vector2 bodyMoveValue;
    public Vector2 cameraMoveValue;
    private InputAction leftLegUpAction;
    private InputAction rightLegUpAction;
    private InputAction leftLegMoveAction;
    private InputAction rightLegMoveAction;
    private InputAction resetButtonAction;
    private InputAction cameraMoveAction;
    private InputAction rotateRightAction;
    private InputAction rotateLeftAction;
    private InputAction bodyMoveAction;
    private InputAction restartGameAction;
    private InputAction launchAction;

    [SerializeField] private CinemachineOrbitalFollow cameraOrbital;
    private Camera mainCamera;
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
    public EControllerInput LaunchInput
    {
        get
        {
            return launchInput;
        }
        set { launchInput = value; }
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
    private void Awake()
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
        restartGameAction = InputSystem.actions.FindAction("ResetGame");
        launchAction = InputSystem.actions.FindAction("Launch");

        leftLegInput = EControllerInput.Down;
        rightLegInput = EControllerInput.Down;
        rightRotateInput = EControllerInput.Up;
        leftRotateInput = EControllerInput.Up;

        mainCamera = Camera.main;
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
        }
        else
        {
            rightLegInput = EControllerInput.Down;
            rightLegMoveValue = Vector2.zero;
        }

        //Pararse
        if (resetButtonAction.IsPressed())
        {
            leftLegInput = EControllerInput.Reset;
            rightLegInput = EControllerInput.Reset;
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

        //Launch

        if(launchAction.IsPressed() && (RightLegActiveState != EBodyActiveState.Ragdoll || LeftLegActiveState != EBodyActiveState.Ragdoll || bodyActiveState != EBodyActiveState.Launch || bodyActiveState != EBodyActiveState.Ragdoll))
        {
            launchInput = EControllerInput.Down;
        }
        else launchInput = EControllerInput.Up;
    }
}
