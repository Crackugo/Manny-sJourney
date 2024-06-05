using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    // Function to enable all children of this GameObject
    public void EnableAllChildren()
    {
        // Iterate through each child transform
        foreach (Transform child in transform)
        {
            // Enable the child GameObject
            child.gameObject.SetActive(true);
        }
    }
}
