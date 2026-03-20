using UnityEngine;
using UnityEngine.Events;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance { get; private set; }

    [Header("Combo Settings")]
    [SerializeField] private int comboMultiplierThreshold = 5;
    [SerializeField] private int maxMultiplier = 8;

    [Header("Events")]
    public UnityEvent<int> onComboUpdated;
    public UnityEvent<int> onMultiplierChanged;
    public UnityEvent onComboReset;

    private int currentCombo = 0;
    private int currentMultiplier = 1;
    private int totalScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterParrySuccess()
    {
        currentCombo++;
        totalScore += 100 * currentMultiplier;

        UpdateMultiplier();

        onComboUpdated.Invoke(currentCombo);

        Debug.Log($"Combo: {currentCombo} | Multiplier: x{currentMultiplier} | Score: {totalScore}");
    }

    public void RegisterParryFail()
    {
        if (currentCombo == 0) return;

        currentCombo = 0;
        currentMultiplier = 1;

        onComboReset.Invoke();
        onComboUpdated.Invoke(currentCombo);
        onMultiplierChanged.Invoke(currentMultiplier);

        Debug.Log($"Combo reset — Score: {totalScore}");
    }

    private void UpdateMultiplier()
    {
        int newMultiplier = Mathf.Clamp(
            1 + (currentCombo / comboMultiplierThreshold),
            1,
            maxMultiplier
        );

        if (newMultiplier != currentMultiplier)
        {
            currentMultiplier = newMultiplier;
            onMultiplierChanged.Invoke(currentMultiplier);
            Debug.Log($"Multiplier increased to x{currentMultiplier}");
        }
    }

    public int GetScore() => totalScore;
    public int GetCombo() => currentCombo;
    public int GetMultiplier() => currentMultiplier;

    public string GetGrade()
    {
        if (totalScore >= 5000) return "S";
        if (totalScore >= 3000) return "A";
        if (totalScore >= 1500) return "B";
        if (totalScore >= 500) return "C";
        return "D";
    }
}