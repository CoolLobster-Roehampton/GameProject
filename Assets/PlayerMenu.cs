using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject finishMenu;
    public GameObject uiCanvas;

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
        uiCanvas.SetActive(false);
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        finishMenu.SetActive(false);
        Time.timeScale = 0f; 
        SetCursorState(false);
    }

    public void ShowPauseMenu()
    {
        uiCanvas.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        SetCursorState(false);
    }

    public void HidePauseMenu()
    {
        uiCanvas.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SetCursorState(true);
    }

    public void ShowFinishMenu()
    {
        uiCanvas.SetActive(false);
        finishMenu.SetActive(true);
        Time.timeScale = 0f;
        SetCursorState(false);
    }

    public void StartGame()
    {
        uiCanvas.SetActive(true);
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
        SetCursorState(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}