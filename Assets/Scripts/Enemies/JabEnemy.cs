using UnityEngine;

public class JabEnemy : EnemyBase
{
    [Header("Jab Settings")]
    [SerializeField] private float jabTelegraphDuration = 0.3f;

    protected override void Start()
    {
        telegraphDuration = jabTelegraphDuration;
        attackDuration = 0.15f;
        recoveryDuration = 0.4f;
        timeBetweenAttacks = 1.5f;
        base.Start();
    }

    protected override void OnTelegraphStart()
    {
        Debug.Log("Jab enemy winding up — tight window incoming");
    }

    protected override void OnAttackStart()
    {
        Debug.Log("JAB — fast attack, tight parry window");
    }
}