using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private Dictionary<GameObject, int> UIs;

    private void Start()
    {
        foreach (GameObject ui in UIs.Keys)
        {
            ui.SetActive(false);
        }
    }
}
