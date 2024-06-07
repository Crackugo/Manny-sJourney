using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string gameSceneName; // Name of the game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName); // Load the scene named "SampleScene"
    }
}