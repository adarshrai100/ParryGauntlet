using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;

    [Header("Combo")]
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Wave")]
    [SerializeField] private TextMeshProUGUI waveText;

    private void Start()
    {
        UpdateScore(0);
        UpdateCombo(0);
        UpdateMultiplier(1);
        UpdateHealth(PlayerHealth.Instance.GetMaxHealth());
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void UpdateCombo(int combo)
    {
        if (comboText == null) return;

        if (combo <= 1)
        {
            comboText.text = "";
            return;
        }

        comboText.text = $"x{combo} COMBO";
    }

    public void UpdateMultiplier(int multiplier)
    {
        if (multiplierText != null)
            multiplierText.text = multiplier > 1 ? $"x{multiplier} MULTIPLIER" : "";
    }

    public void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = $"Health: {health}";
    }

    public void UpdateWave(int wave)
    {
        if (waveText != null)
            waveText.text = $"Wave {wave}";
    }
}