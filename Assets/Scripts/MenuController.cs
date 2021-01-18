using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        Main,
        Settings,
        SelectLevel
    }

    public CanvasGroup mainScreen;
    public CanvasGroup settingsScreen;
    public CanvasGroup selectLevelScreen;

    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
        Utility.SetCanvasGroupEnabled(selectLevelScreen, screen == Screen.SelectLevel);
    }
    
    void Start()
    {
        OpenMainMenu();
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OpenMainMenu()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void OpenSettings()
    {
        SetCurrentScreen(Screen.Settings);
    }

    public void OpenLevelSelect()
    {
        SetCurrentScreen(Screen.SelectLevel);
    }

    public void ChooseLevel(string levelSceneName)
    {
        SceneManager.LoadScene(levelSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
