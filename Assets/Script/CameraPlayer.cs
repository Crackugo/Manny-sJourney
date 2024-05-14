using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    // Speed of camera rotation and zoom out
    public float rotationSpeed = 30.0f;
    public float zoomSpeed = 5.0f;

    // Duration of rotation and zoom out
    public float rotationDuration = 1.5f;
    public float zoomDuration = 3.0f;

    private bool isRotating = true;
    // Initial rotation of the camera
    private Quaternion initialRotation;

    public Transform player; // Reference to the player's transform
    public float distance = 8.0f; // Distance from the player
    public float sensitivity = 2.0f; // Mouse sensitivity
    public Vector2 pitchMinMax = new Vector2(-20, 85); // Pitch limits

    private float yaw = 0.0f; // Yaw angle
    private float pitch = 0.0f; // Pitch angle

    void Start()
    {
        // Save initial rotation of the camera
        initialRotation = transform.rotation;

        // Start the camera movement routine
        StartCoroutine(StartCameraMovement());
    }

    IEnumerator StartCameraMovement()
    {
        // Rotate the camera 90 degrees
        float rotationAmount = 90.0f;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(rotationAmount, 0, 0);
        float rotationTimer = 0.0f;

        while (rotationTimer < rotationDuration)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        // Zoom out gradually
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 targetPosition = player.position + direction;
        float zoomTimer = 0.0f;

        while (zoomTimer < zoomDuration)
        {
            zoomTimer += Time.deltaTime;
            float t = Mathf.Clamp01(zoomTimer / zoomDuration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            yield return null;
        }
        isRotating = false;
    }

    void LateUpdate()
    {
        if (player != null && !isRotating)
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