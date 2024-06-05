using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private float speed;

    private CloudSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<CloudSpawner>();
        speed = Random.Range(30f, 60f);
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
