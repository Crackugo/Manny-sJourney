using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject fallingPlatforms; // Assign the parent object in the Inspector

    public void ResetAllPlatforms()
    {
        foreach (Transform platform in fallingPlatforms.transform)
        {
            FallingPlatform fallingPlatform = platform.GetComponent<FallingPlatform>();
            if (fallingPlatform != null)
            {
                fallingPlatform.ResetPlatform();
            }
        }
    }
}
