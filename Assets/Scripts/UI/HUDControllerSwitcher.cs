using UnityEngine;

public class HUDControlSwitcher : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    [SerializeField] private GameObject keyboardPanel;
    [SerializeField] private GameObject gamepadPanel;

    public void ChangeHUD(string deviceName)
    {
        if (deviceName == "Gamepad")
        {
            gamepadPanel.SetActive(true);
            keyboardPanel.SetActive(false);
        }
        else if (deviceName == "Keyboard")
        {
            keyboardPanel.SetActive(true);
            gamepadPanel.SetActive(false);
        }
    }
}