using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform player;
    public float shootInterval = 2f;
    public float projectileSpeed = 8f;

    private void Start()
    {
        InvokeRepeating("ShootProjectile", 0f, shootInterval);
    }

    private void ShootProjectile()
    {
        
        Vector3 direction = (player.position - transform.position).normalized;

        
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on the projectile prefab.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guy"))
        {
            Destroy(gameObject); 
        }
        
    }
}
