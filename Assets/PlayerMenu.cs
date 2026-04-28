using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject finishMenu;

    void Start()
    {
        ShowMainMenu();
    }

    void SetCursorState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        finishMenu.SetActive(false);
        Time.timeScale = 0f; 
        SetCursorState(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        SetCursorState(false);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SetCursorState(true);
    }

    public void ShowFinishMenu()
    {
        finishMenu.SetActive(true);
        Time.timeScale = 0f;
        SetCursorState(false);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
        SetCursorState(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}