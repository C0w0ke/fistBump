using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelection : MonoBehaviour
{
    public void SelectStage(string sceneName)
    {
        Debug.Log("Selected stage!");
        SceneManager.LoadScene(sceneName);
    }
}
