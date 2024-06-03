using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    private MovingPlatform[] movingPlatforms; // Array to hold references to all moving platforms

    void Start()
    {
        // Find all MovingPlatform components in the children of this GameObject
        movingPlatforms = GetComponentsInChildren<MovingPlatform>();
    }

    // Method to reset all platforms to their start positions
    public void ResetAllPlatforms()
    {
        foreach (MovingPlatform platform in movingPlatforms)
        {
            platform.ResetPlatform();
        }
    }
}
