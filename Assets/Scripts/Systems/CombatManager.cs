using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private EnemyBase currentAttacker;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool CanAttack(EnemyBase enemy)
    {
        return currentAttacker == null || currentAttacker == enemy;
    }

    public bool RequestAttack(EnemyBase enemy)
    {
        if (currentAttacker != null && currentAttacker != enemy)
            return false;

        currentAttacker = enemy;
        currentAttacker.SetActiveIndicator(true);

        return true;
    }

    public void ReleaseAttack(EnemyBase enemy)
    {
        if (currentAttacker == enemy)
        {
            currentAttacker.SetActiveIndicator(false);
            currentAttacker = null;
        }
    }

    public EnemyBase GetCurrentAttacker()
    {
        return currentAttacker;
    }
}