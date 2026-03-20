using UnityEngine;

public class OverheadEnemy : EnemyBase
{
    [Header("Overhead Settings")]
    [SerializeField] private float overheadTelegraphDuration = 1.2f;

    protected override void Start()
    {
        telegraphDuration = overheadTelegraphDuration;
        attackDuration = 0.25f;
        recoveryDuration = 0.8f;
        timeBetweenAttacks = 2.5f;
        base.Start();
    }

    protected override void OnTelegraphStart()
    {
        Debug.Log("Overhead enemy winding up — slow and obvious, wide parry window");
    }

    protected override void OnAttackStart()
    {
        Debug.Log("OVERHEAD — slow attack, generous parry window");
    }
}