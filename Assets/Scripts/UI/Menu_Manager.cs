using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField]
    private string GameScene_name;
    [SerializeField]
    private GameObject[] sub_menus;

    public void LaunchGame()
    {
        SceneManager.LoadScene(GameScene_name);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            foreach (GameObject menu in sub_menus) menu.SetActive(false);
        }
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
