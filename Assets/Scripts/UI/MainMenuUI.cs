using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject titleScreenPanel;
    public GameObject rulesPanel;
    public GameObject creditsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Scene");
    }

    public void ToggleRules()
    {
        if (rulesPanel != null)
        {
            titleScreenPanel.SetActive(false);
            rulesPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }
    }

    public void ToggleCredits()
    {
        if (creditsPanel != null)
        {
            titleScreenPanel.SetActive(false);
            rulesPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }
    }

    public void ClosePanels()
    {
        if(titleScreenPanel != null) titleScreenPanel.SetActive(true);
        if (rulesPanel != null) rulesPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
