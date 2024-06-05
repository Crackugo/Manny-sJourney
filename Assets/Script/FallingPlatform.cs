using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float fallSpeed = 10f;
    public float delayBeforeFall = 1f;

    private Vector3 initialPosition;
    private Coroutine fallingCoroutine;

    private void Start()
    {
        // Store the initial position of the platform
        initialPosition = transform.position;
    }

    public void StartFalling()
    {
        // Start the falling coroutine
        if (fallingCoroutine == null)
        {
            fallingCoroutine = StartCoroutine(StartFallingCoroutine());
        }
    }

    public void ResetPlatform()
    {
        // Stop the falling coroutine if it is running
        if (fallingCoroutine != null)
        {
            StopCoroutine(fallingCoroutine);
            fallingCoroutine = null;
        }

        // Reset the position of the platform to the initial position
        transform.position = initialPosition;
    }

    private IEnumerator StartFallingCoroutine()
    {
        yield return new WaitForSeconds(delayBeforeFall);

        while (true)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
