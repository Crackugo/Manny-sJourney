using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public EventReference lightningEvent;
    public Transform playerTransform; // Reference to the player's transform
    public float maxDistance = 20f; // Maximum distance at which the sound is audible
    public float minVolume = 0.1f; // Minimum volume of the sound
    public float maxVolume = 1f; // Maximum volume of the sound
    public float lightningInterval = 6f; // Interval between lightning sounds in seconds

    public bool isLightning = false;
    private List<Transform> childrenTransforms; // List to store references to children transforms
    private float lastLightningTime; // Timestamp of the last lightning sound

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the list of children transforms
        childrenTransforms = new List<Transform>();

        // Get references to all child transforms
        foreach (Transform child in transform)
        {
            childrenTransforms.Add(child);
        }

        // Initialize last lightning time
        lastLightningTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.y >= 90 && playerTransform.position.y <= 110 && playerTransform.position.z<=20)
        {
            isLightning = false;
        }
        else
        {
            isLightning = true;
        }
        if (playerTransform.position.y <=135 && playerTransform.position.y >= 110 && (playerTransform.position.x < 65 || playerTransform.position.x > 110 || playerTransform.position.z<-50 || playerTransform.position.z>-10))
        {
            isLightning = false;
        }
        // Check if it's time for the next lightning sound
        if (Time.time - lastLightningTime >= lightningInterval && playerTransform.position.y >= 90 && isLightning)
        {
            // Loop through each child and play the lightning sound
            foreach (Transform childTransform in childrenTransforms)
            {
                float distanceToPlayer = Vector3.Distance(childTransform.position, playerTransform.position);

                // Calculate volume based on distance
                float volume = Mathf.Lerp(maxVolume, minVolume, distanceToPlayer / maxDistance);
                volume = Mathf.Clamp01(volume); // Clamp volume between 0 and 1

                Debug.Log("Distance to Player: " + distanceToPlayer + ", Volume: " + volume);

                // Play the lightning event sound with adjusted volume
                FMODUnity.RuntimeManager.PlayOneShot(lightningEvent, childTransform.position);
            }

            // Update last lightning time
            lastLightningTime = Time.time;
        }
    }
}