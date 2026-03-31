using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float hitCooldown = 1f;

    private float lastHitTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        // Try to get PlayerHealth from the player
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            lastHitTime = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            lastHitTime = Time.time;
        }
    }
}