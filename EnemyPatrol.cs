using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Attack,
        Hit,
        Dead
    }

    [Header("Patrol")]
    public float waitTime = 2f;
    private float waitCounter;
    private bool isWaiting = false;

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Combat")]
    public float attackRange = 2f;
    public Transform player;

    public int health = 3;

    public GameObject attackHitbox;

    public float attackCooldown = 2f;
    private float attackTimer;
    private bool isAttacking = false;

    private Transform target;
    private EnemyState currentState;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        target = pointB;
        currentState = EnemyState.Patrol;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        waitCounter = 0;
        attackTimer = 0;
    }

    void Update()
    {
        Debug.Log(
    "isWalking: " + animator.GetBool("isWalking") +
    " | isAttacking: " + animator.GetBool("isAttacking")
            );

        if (currentState == EnemyState.Dead)
            return;

        attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        
        

        // ✅ Attack logic
        if (distanceToPlayer < attackRange)
        {
            FlipToTarget(player);

            if (attackTimer <= 0 && !isAttacking)
            {
                StartAttack();
            }
            else
            {
                currentState = EnemyState.Attack;
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        // Patrol logic (timers only)
        if (currentState == EnemyState.Patrol)
        {
            PatrolLogic();
        }
    }

    void FixedUpdate()
    {
        if (currentState != EnemyState.Patrol)
            return;

        PatrolMovement();
    }

    // ---------------- PATROL ----------------

    void PatrolLogic()
    {
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            animator.SetBool("isWalking", false);

            if (waitCounter <= 0)
            {
                isWaiting = false;
                SwitchTarget();
            }
        }
    }

    void PatrolMovement()
    {
        if (isWaiting)
            return;

        // Only walk while actually moving
        animator.SetBool("isWalking", true);

        FlipToTarget(target);

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            target.position,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, target.position) < 0.5f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    void SwitchTarget()
    {
        target = (target == pointA) ? pointB : pointA;

        isWaiting = false;
        Debug.Log("Walking: " + animator.GetBool("isWalking"));
        waitCounter = 0;
    }

    // ---------------- ATTACK ----------------

    void StartAttack()
    {
        currentState = EnemyState.Attack;
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;  // stop movement

        FlipToTarget(player);

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        Invoke("EnableHitbox", 0.3f);
        Invoke("DisableHitbox", 0.6f);
        Invoke("EndAttack", 1f);
    }

    void EndAttack()
    {
        isAttacking = false;
        attackTimer = attackCooldown;

        animator.SetBool("isAttacking", false);

        currentState = EnemyState.Patrol;
    }

    void EnableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    // ---------------- DAMAGE ----------------

    public void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Dead)
            return;

        health -= damage;

        animator.SetTrigger("Hit");
        currentState = EnemyState.Hit;

        CancelInvoke("RecoverFromHit");
        Invoke("RecoverFromHit", 0.4f);

        if (health <= 0)
        {
            Die();
        }
    }

    void RecoverFromHit()
    {
        if (currentState != EnemyState.Dead)
            currentState = EnemyState.Patrol;
    }

    // ---------------- DEATH ----------------

    public void Die()
    {
        currentState = EnemyState.Dead;

        animator.SetTrigger("Die");

        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 1f);
    }

    // ---------------- FLIP ----------------

    void FlipToTarget(Transform target)
    {
        Vector3 scale = transform.localScale;

        if (target.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}