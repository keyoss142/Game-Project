using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Try normal enemy first
            EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                return;
            }

            // Try boss enemy
            BossEnemy boss = other.GetComponent<BossEnemy>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }
    }
}