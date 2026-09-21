using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float telegraphDuration = 0.8f;
    [SerializeField] protected float attackDuration = 0.3f;
    [SerializeField] protected float recoveryDuration = 0.5f;
    [SerializeField] protected float timeBetweenAttacks = 2f;

    [Header("References")]
    [SerializeField] protected ParrySystem playerParrySystem;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform playerTransform;

    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float attackRange = 1.5f;

    [Header("Attack Timing")]
    [SerializeField] protected float minAttackDelay = 0.2f;
    [SerializeField] protected float maxAttackDelay = 0.7f;

    [Header("Enemy Health")]
    [SerializeField] protected int maxHealth = 3;
    protected int currentHealth;

    public enum EnemyState { Idle, Telegraph, Attack, Recovery }
    protected EnemyState currentState = EnemyState.Idle;
    private Vector3 waitingPosition;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if (playerParrySystem == null)
        {
            playerParrySystem = FindObjectOfType<ParrySystem>();
            Debug.Log($"ParrySystem auto-found: {playerParrySystem != null}");
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError($"{gameObject.name}: Player with tag 'Player' not found!");
            }
        }
        waitingPosition = transform.position;
        StartCoroutine(AttackLoop());
    }

    protected virtual IEnumerator MoveToAttackRange()
    {
        while (playerTransform != null)
        {
            float distance = Mathf.Abs(transform.position.x - playerTransform.position.x);

            if (distance <= attackRange)
            {
                yield break;
            }

            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);

            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

            // Face the player
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1f : -1f);
            transform.localScale = scale;

            yield return null;
        }
    }

    protected virtual IEnumerator AttackLoop()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);

            // Wait until this enemy gets the attack turn
            while (!CombatManager.Instance.RequestAttack(this))
            {
                yield return null;
            }

            // This enemy is now the ONLY one allowed to approach
            yield return StartCoroutine(MoveToAttackRange());

            float attackDelay = Random.Range(minAttackDelay, maxAttackDelay);
            yield return new WaitForSeconds(attackDelay);

            yield return StartCoroutine(DoAttackSequence());
            

        }
    }


    protected virtual IEnumerator DoAttackSequence()
    {
        yield return StartCoroutine(Telegraph());
        yield return StartCoroutine(Attack());
        yield return StartCoroutine(Recovery());
    }

    protected virtual IEnumerator Telegraph()
    {
        currentState = EnemyState.Telegraph;
        OnTelegraphStart();
        yield return new WaitForSeconds(telegraphDuration);
    }

    protected virtual IEnumerator Attack()
    {
        currentState = EnemyState.Attack;

        Debug.Log($"{gameObject.name} ENTERED ATTACK STATE");

        OnAttackStart();

        playerParrySystem.OpenParryWindow(this);

        yield return new WaitForSeconds(attackDuration);
    }

    protected virtual IEnumerator Recovery()
    {
        currentState = EnemyState.Recovery;
        OnRecoveryStart();
        yield return new WaitForSeconds(recoveryDuration);
        currentState = EnemyState.Idle;
    }

    protected virtual void OnTelegraphStart()
    {
        Debug.Log($"{gameObject.name} telegraphing attack");
    }

    protected virtual void OnAttackStart()
    {
        Debug.Log("========== ON ATTACK START ==========");

        if (animator == null)
        {
            Debug.LogError($"{gameObject.name}: Animator reference is NULL!");
            return;
        }

        Debug.Log($"{gameObject.name}: Animator found -> {animator.gameObject.name}");

        animator.SetTrigger("Attack");

        Debug.Log($"{gameObject.name}: Attack trigger SENT");
    }

    protected virtual void OnRecoveryStart()
    {
        Debug.Log($"{gameObject.name} recovering");
    }

    public virtual void TakeParryDamage(int damage = 1)
    {
        currentHealth -= damage;

        Debug.Log($"{gameObject.name} parried! Enemy health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        CombatManager.Instance.ReleaseAttack(this);

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Debug.Log($"{gameObject.name} defeated!");

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(0.75f);

        WaveManager.Instance.ReportEnemyDead();
        Destroy(gameObject);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        timeBetweenAttacks /= multiplier;
        telegraphDuration /= multiplier;
    }

    public void SetPlayerParrySystem(ParrySystem parrySystem)
    {
        playerParrySystem = parrySystem;
    }

    public EnemyState GetCurrentState() => currentState;
}