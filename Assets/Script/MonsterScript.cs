using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    // The target the object will follow
    public Transform target;

    // The speed at which the object will follow the target
    public float followSpeed = 5f;

    // The distance at which the object will stop following the target
    public float stopDistance = 1f;

    // The speed at which the object will rotate to face the target
    public float rotationSpeed = 5f;

    // Variables to store the initial position and rotation
    private bool kill;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Check if the target is not null
        if (target != null && kill)
        {
            // Calculate the distance to the target
            float distance = Vector3.Distance(transform.position, target.position);

            // Check if the distance is greater than the stop distance
            if (distance > stopDistance)
            {
                // Calculate the direction to the target
                Vector3 direction = (target.position - transform.position).normalized;

                // Move the object towards the target
                transform.position += direction * followSpeed * Time.deltaTime;
            }

            // Calculate the direction to look at the target
            Vector3 lookDirection = target.position - transform.position;
            lookDirection.y = 0; // Keep the rotation on the y-axis

            if (lookDirection != Vector3.zero)
            {
                // Calculate the rotation to look at the target
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // Smoothly rotate towards the target
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Function to start the movement behind the target
    public void startKilling(float distanceBehind, float time)
    {
        StartCoroutine(StartKillingCoroutine(distanceBehind, time));
    }

    // Coroutine to handle the delay before starting the movement
    private IEnumerator StartKillingCoroutine(float distanceBehind, float time)
    {
        yield return new WaitForSeconds(time); // Wait for 4 seconds
        kill = true;
    }

    // Function to elevate the monster by a specified amount
    public void ElevateBy(float elevation)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + elevation, transform.position.z);
    }

    // Function to reset the position and rotation of the monster
    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        kill = false;
    }
}
