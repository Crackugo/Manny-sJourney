using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint; // The starting point of the platform
    public Transform endPoint;   // The ending point of the platform
    public float speed = 2.0f;   // Speed of the platform

    private Vector3 _startPos;   // Initial position of the platform
    private Vector3 _endPos;     // End position of the platform
    private bool _movingToEnd = true; // Flag to indicate direction of movement

    void Start()
    {
        // Store initial and end positions
        _startPos = startPoint.position;
        _endPos = endPoint.position;
    }

    void Update()
    {
        // Move the platform between start and end points
        if (_movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPos, speed * Time.deltaTime);
            if (transform.position == _endPos)
                _movingToEnd = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, speed * Time.deltaTime);
            if (transform.position == _startPos)
                _movingToEnd = true;
        }
    }
}
