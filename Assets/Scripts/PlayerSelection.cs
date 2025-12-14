using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{

    public GameObject Characters;
    private GameObject[] allCharacters;
    private int currentIndex = 0;

    void Start()
    {
        allCharacters = new GameObject[Characters.transform.childCount];

        for (int i = 0; i < Characters.transform.childCount; i++)
        {
            allCharacters[i] = Characters.transform.GetChild(i).gameObject;
            allCharacters[i].SetActive(false);
        }

        if (PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        }

        ShowCurrentCharacter();
    }

    void ShowCurrentCharacter()
    {
        foreach (GameObject character in allCharacters)
        {
            character.SetActive(false);
        }

        allCharacters[currentIndex].SetActive(true);
    }

    public void NextCharacter()
    {
        Debug.Log("Next Button Pressed!");
        currentIndex = (currentIndex + 1) % allCharacters.Length;
        ShowCurrentCharacter();
    }

    public void PreviousCharacter()
    {
        Debug.Log("Previous Button Pressed!");
        currentIndex = (currentIndex - 1 + allCharacters.Length) % allCharacters.Length;
        ShowCurrentCharacter();
    }

    public void SelectCharacter(string sceneName)
    {
        Debug.Log("Select number " + currentIndex + " character!");
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName);
    }
}
