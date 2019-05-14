using UnityEngine;

public class HealthController : MonoBehaviour
{

    [SerializeField] private float health = 100f;

    public void ApplyDamage(float damage)
    {
        health -= damage;
        
        if(health <= 0)
        {
            health = 0f;
            Destroy(gameObject);
        }
    }
}
