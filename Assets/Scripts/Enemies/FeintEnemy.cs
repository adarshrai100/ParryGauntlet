using System.Collections;
using UnityEngine;

public class FeintEnemy : EnemyBase
{
    [Header("Feint Settings")]
    [SerializeField] private float feintChance = 0.5f;
    [SerializeField] private float feintTelegraphDuration = 0.4f;

    protected override void Start()
    {
        telegraphDuration = feintTelegraphDuration;
        attackDuration = 0.2f;
        recoveryDuration = 0.6f;
        timeBetweenAttacks = 2f;
        base.Start();
    }

    protected override IEnumerator DoAttackSequence()
    {
        yield return StartCoroutine(Telegraph());

        if (Random.value < feintChance)
        {
            yield return StartCoroutine(Feint());
            yield return StartCoroutine(Telegraph());
        }

        yield return StartCoroutine(Attack());
        yield return StartCoroutine(Recovery());
    }

    private IEnumerator Feint()
    {
        Debug.Log("FEINT — fake out, resetting...");
        currentState = EnemyState.Idle;
        yield return new WaitForSeconds(0.3f);
    }

    protected override void OnTelegraphStart()
    {
        Debug.Log("Feint enemy winding up — might be a fake");
    }

    protected override void OnAttackStart()
    {
        Debug.Log("FEINT ENEMY REAL ATTACK — this one counts");
    }
}