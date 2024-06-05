using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 100f; // Speed of the spin, adjustable in the Inspector
    public Vector3 axis = Vector3.up; // Axis of rotation, adjustable in the Inspector

    void Update()
    {
        // Rotate the object around the specified axis at the specified speed
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
