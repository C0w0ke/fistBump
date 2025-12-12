using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Text resultText;

    public FightingController[] fightingControllers;
    public OpponentAI[] opponentAI_s;

    private void Update()
    {
        foreach (FightingController fightingController in fightingControllers)
        {
            if (fightingController.gameObject.activeSelf && fightingController.currentHealth <= 0)
            {
                SetResult("You lose!");
                return;
            }
        }
        foreach (OpponentAI opponentAI in opponentAI_s)
        {
            if (opponentAI.gameObject.activeSelf && opponentAI.currentHealth <= 0)
            {
                SetResult("You win!");
                return;
            }
        }
    }

    void SetResult(string result)
    {
        resultText.text = result;
        resultPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
