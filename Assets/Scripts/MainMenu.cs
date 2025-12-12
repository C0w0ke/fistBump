using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject characterAndStageMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        characterAndStageMenu.SetActive(false);
    }

    public void PlayButtonClicked()
    {
        mainMenu.SetActive(false);
        characterAndStageMenu.SetActive(true);
    }

    public void OptionsButtonClicked()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ControlsButtonClicked()
    {
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void BackButtonClicked()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        characterAndStageMenu.SetActive(false);
    }

    public void SelectCharacterClicked(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SelectStageClicked(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

}
