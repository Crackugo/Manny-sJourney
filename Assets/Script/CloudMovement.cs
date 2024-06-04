using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2f;

    private CloudSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<CloudSpawner>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        
        if (transform.position.x > 600f)  // Adjust based on your screen bounds
        {
            Destroy(gameObject);
            spawner.CloudDestroyed();
        }
    }
}
