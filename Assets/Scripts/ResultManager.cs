using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Text resultText;

    private void Update()
    {
        BaseCharacter[] allActiveCharacters = FindObjectsOfType<BaseCharacter>();

        if (allActiveCharacters.Length == 0)
        {
            Debug.LogWarning("ResultManager: No active BaseCharacter instances found in scene!");
            return;
        }

        foreach (BaseCharacter character in allActiveCharacters)
        {
            if (character != null && character.gameObject.activeSelf && character.healthComponent != null)
            {
                if (character.healthComponent.currentHealth <= 0)
                {
                    string result = (character is FightingController) ? "You lose!" : "You win!";
                    SetResult(result);
                    return;
                }
            }
            else
            {
                Debug.LogWarning("ResultManager: Skipping inactive/null character or missing healthComponent.");
            }
        }
    }

    void SetResult(string result)
    {
        if (resultPanel == null || resultText == null)
        {
            Debug.LogError("ResultManager: resultPanel or resultText not assigned!");
            return;
        }
        resultText.text = result;
        resultPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("ResultManager: " + result);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
