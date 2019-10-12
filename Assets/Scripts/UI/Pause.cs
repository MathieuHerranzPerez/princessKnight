using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private string MainMenuScene = "Menu";
    [SerializeField]
    private GameObject PauseMenu;
    public Action GameResumed;
    public Action GamePaused;
    private float timescale;

    private void Start()
    {
        PauseMenu.SetActive(false);
        timescale = Time.timeScale;
    }

    public void OpenClosePause()
    {
        if (PauseMenu.activeInHierarchy)
        {
            PauseMenu.SetActive(false);
            GameResumed?.Invoke();
            Time.timeScale = timescale;
        }
        else
        {
            PauseMenu.SetActive(true);
            GameResumed?.Invoke();
            timescale = Time.timeScale;
            Time.timeScale = 0.0f;
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OpenClosePause();
        }
    }
}
