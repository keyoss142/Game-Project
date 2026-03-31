using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Attack,
        Hit,
        Dead
    }

    public Transform player;
    public float speed = 2f;
    public float attackRange = 2f;

    public int health = 10;

    public float attackCooldown = 2f;
    private float attackTimer;

    public float blockChance = 0.2f;

    public Transform leftLimit;
    public Transform rightLimit;

    public GameObject attackHitbox;

    private EnemyState currentState;
    private Animator animator;

    private bool isAttacking = false;

    private bool canTakeDamage = true;
    public float hitCooldown = 0.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        if (currentState == EnemyState.Dead || currentState == EnemyState.Hit)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            currentState = EnemyState.Patrol;
        }
        else
        {
            if (!isAttacking && attackTimer <= 0)
            {
                DecideAction();
            }
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                MoveToPlayer();
                break;
        }

        attackTimer -= Time.deltaTime;
    }

    void MoveToPlayer()
    {
        animator.SetBool("isWalking", true);
        

        float targetX = player.position.x;

        // 🔒 Clamp boss inside area
        targetX = Mathf.Clamp(targetX, leftLimit.position.x, rightLimit.position.x);

        Vector2 targetPos = new Vector2(targetX, transform.position.y);

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        FlipTowardsPlayer();
    }

    void DecideAction()
    {
        int attackType = Random.Range(0, 2);

        if (attackType == 0)
            Attack1();
        else
            Attack2();
    }

    void Attack1()
    {
        currentState = EnemyState.Attack;
        isAttacking = true;

        animator.SetTrigger("Attack1");
        animator.SetBool("isWalking", false);

        Invoke("EnableHitbox", 0.3f);
        Invoke("DisableHitbox", 0.6f);

        Invoke("EndAttack", 1f);
    }

    void Attack2()
    {
        currentState = EnemyState.Attack;
        isAttacking = true;

        animator.SetTrigger("Attack2");
        animator.SetBool("isWalking", false);

        Invoke("EnableHitbox", 0.4f);
        Invoke("DisableHitbox", 0.8f);

        Invoke("EndAttack", 1.2f);
    }

    void EndAttack()
    {
        isAttacking = false;
        attackTimer = attackCooldown;
        currentState = EnemyState.Patrol;
    }

    void EnableHitbox()
    {
        attackHitbox.SetActive(true);
    }

    void DisableHitbox()
    {
        attackHitbox.SetActive(false);
    }
    void ResetDamage()
    {
        canTakeDamage = true;
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage || currentState == EnemyState.Dead)
            return;

        canTakeDamage = false;

        health -= damage;

        if (health <= 0)
        {
            Die();
            return;
        }

        animator.SetTrigger("Hit");
        currentState = EnemyState.Hit;

        Invoke("ResetDamage", hitCooldown);
        Invoke("RecoverFromHit", 0.3f);
    }

    void RecoverFromHit()
    {
        if (currentState != EnemyState.Dead)
            currentState = EnemyState.Patrol;
    }

    void Die()
    {
        currentState = EnemyState.Dead;
        animator.SetTrigger("Die");

        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2f);
    }

    void FlipTowardsPlayer()
    {
        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);   // face right
        else
            scale.x = -Mathf.Abs(scale.x);  // face left

        transform.localScale = scale;
    }
}