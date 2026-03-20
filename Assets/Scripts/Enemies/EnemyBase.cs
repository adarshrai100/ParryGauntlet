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

    public enum EnemyState { Idle, Telegraph, Attack, Recovery }
    protected EnemyState currentState = EnemyState.Idle;

    protected virtual void Start()
    {
        StartCoroutine(AttackLoop());
    }

    protected virtual IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
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
        OnAttackStart();
        playerParrySystem.OpenParryWindow();
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
        Debug.Log($"{gameObject.name} attacking");
    }

    protected virtual void OnRecoveryStart()
    {
        Debug.Log($"{gameObject.name} recovering");
    }

    public EnemyState GetCurrentState() => currentState;
}