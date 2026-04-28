using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject finishMenu;
    public GameObject uiCanvas;
    public GameObject dialougeBox;
    public TextMeshProUGUI dialougeText;
    void Start()
    {
        ShowMainMenu();
        dialougeBox.SetActive(false);
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

    public void ShowDialouge(string text)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDialouge(text));
    }

    IEnumerator AnimateDialouge(string text)
    {
        dialougeBox.SetActive(true);
        dialougeText.text = text;

        CanvasGroup cg = dialougeBox.GetComponent<CanvasGroup>();
        if (cg == null) { cg = dialougeBox.AddComponent<CanvasGroup>(); }

        float t = 0f;
        float duration = 0.25f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = t / duration;

            cg.alpha = p;
            dialougeBox.transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, p);
            yield return null;
        }

        cg.alpha = 1f;

        yield return new WaitForSecondsRealtime(2.5f);

        t = 0f;
        while ( t < duration )
        {
            t += Time.unscaledDeltaTime;
            float p = t / duration;

            cg.alpha = 1f - p;

            yield return null;
        }

        cg.alpha = 0f;
        dialougeBox.SetActive(false);

    }
}