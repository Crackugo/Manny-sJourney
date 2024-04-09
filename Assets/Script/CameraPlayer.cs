using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    public Transform player; // Reference to the player's transform
    public float distance = 5.0f; // Distance from the player
    public float sensitivity = 2.0f; // Mouse sensitivity
    public Vector2 pitchMinMax = new Vector2(-20, 85); // Pitch limits

    private float yaw = 0.0f; // Yaw angle
    private float pitch = 0.0f; // Pitch angle

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate yaw and pitch angles based on mouse input
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            // Calculate the position of the camera relative to the player
            Vector3 direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 position = player.position + rotation * direction;

            // Set the camera's position and rotation
            transform.position = position;
            transform.LookAt(player);
        }
    }
}
