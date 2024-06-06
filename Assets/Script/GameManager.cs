using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public CameraPlayer cameraPlayer; // Reference to the CameraPlayer script
    public Text tutorialText; // Reference to the UI Text component for messages

    public float messageDisplayTime = 3.0f; // Time to display each message
    private string[] tutorialMessages = {
        "Welcome to the Game!",
        "Use WASD to move around.",
        "Press Space to jump.",
        "Avoid obstacles and reach the end!",
        "Good luck!"
    };

    void Start()
    {
        StartCoroutine(ShowTutorialMessages());
    }

     IEnumerator ShowTutorialMessages()
    {
        // Disable player controls
        playerMovement.canMove = false;
        cameraPlayer.canControl = false;

        foreach (string message in tutorialMessages)
        {
            tutorialText.text = message;
            yield return new WaitForSeconds(messageDisplayTime);
        }

        // Clear the message
        tutorialText.text = "";

        // Enable player controls
        playerMovement.canMove = true;
        cameraPlayer.canControl = true;
    }
}