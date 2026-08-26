using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] UIs;

    private void Start()
    {
        ReturnMainMenu();
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= UIs.Length) return;

        for (int i = 0; i < UIs.Length; i++)
        {
            UIs[i].SetActive(false);
        }

        for (int i = 0; i < UIs.Length; i++)
        {
            UIs[i].SetActive(i == index);
        }
    }

    public void ReturnMainMenu()
    {
        ShowPanel(0);
    }
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}

