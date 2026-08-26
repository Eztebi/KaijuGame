using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject keyboardScheme;
    [SerializeField] private GameObject gamepadScheme;
    private void OnEnable()
    {
        if (Gamepad.current != null)
        {
            keyboardScheme.SetActive(false);
            gamepadScheme.SetActive(true);
        }
        else
        {
            keyboardScheme.SetActive(true);
            gamepadScheme.SetActive(false);
        }
    }
}
