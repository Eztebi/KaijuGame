using TMPro;
using UnityEngine;

public class HUDControlSwitcher : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    [SerializeField] private GameObject keyboardPanel;
    [SerializeField] private GameObject gamepadPanel;
    [SerializeField] private TextMeshProUGUI timer;
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
    private void Update()
    {
        float time = Time.time;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}