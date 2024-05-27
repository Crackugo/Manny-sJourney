using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint; // The starting point of the platform
    public Transform endPoint;   // The ending point of the platform
    public float speed = 2.0f;   // Speed of the platform

    private Vector3 _startPos;   // Initial position of the platform
    private Vector3 _endPos;     // End position of the platform
    private bool _movingToEnd = true; // Flag to indicate direction of movement
    private float _threshold = 0.1f; // Threshold to determine if the platform has reached its destination

    private Vector3 _previousPosition; // To track the previous position of the platform

    void Start()
    {
        // Store initial and end positions
        _startPos = startPoint.position;
        _endPos = endPoint.position;
        _previousPosition = transform.position;
    }

    void Update()
    {
        // Move the platform between start and end points
        if (_movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _endPos) < _threshold)
                _movingToEnd = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _startPos) < _threshold)
                _movingToEnd = true;
        }

        // Calculate the difference in position
        Vector3 movement = transform.position - _previousPosition;

        // Update the previous position
        _previousPosition = transform.position;

        // If the platform has children, move them accordingly
        foreach (Transform child in transform)
        {
            CharacterController controller = child.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Move the child object (player) with the platform
                controller.Move(movement);
            }
            else
            {
                // For non-CharacterController children, move them directly
                child.position += movement;
            }
        }
    }
}
