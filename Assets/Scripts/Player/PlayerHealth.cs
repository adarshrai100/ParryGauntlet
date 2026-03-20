using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;

    [Header("Events")]
    public UnityEvent<int> onHealthChanged;
    public UnityEvent onPlayerDeath;

    private int currentHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged.Invoke(currentHealth);
    }

    public void TakeDamage(int amount = 1)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        CameraShake.Instance.AddTrauma(0.7f);

        onHealthChanged.Invoke(currentHealth);
        Debug.Log($"Player hit — health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        Debug.Log("Player died — game over");
        onPlayerDeath.Invoke();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}