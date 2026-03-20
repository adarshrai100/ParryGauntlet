using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Wave Configs")]
    [SerializeField] private List<WaveConfigSO> waves = new List<WaveConfigSO>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform playerTransform;

    [Header("Events")]
    public UnityEvent<int> onWaveStarted;
    public UnityEvent<int> onWaveCompleted;
    public UnityEvent onAllWavesCompleted;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

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
        StartCoroutine(StartWave(currentWaveIndex));
    }

    private IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            onAllWavesCompleted.Invoke();
            Debug.Log("All waves completed!");
            yield break;
        }

        WaveConfigSO wave = waves[waveIndex];
        isSpawning = true;

        Debug.Log($"Wave {waveIndex + 1} starting: {wave.waveName}");
        onWaveStarted.Invoke(waveIndex + 1);

        yield return new WaitForSeconds(wave.waveStartDelay);

        foreach (var spawnData in wave.enemies)
        {
            yield return new WaitForSeconds(spawnData.spawnDelay > 0
                ? spawnData.spawnDelay
                : wave.timeBetweenSpawns);

            SpawnEnemy(spawnData.enemyPrefab, wave.speedMultiplier);
        }

        isSpawning = false;
    }

    private void SpawnEnemy(GameObject prefab, float speedMultiplier)
    {
        if (prefab == null) return;

        Vector3 spawnPos = spawnPoint != null
            ? spawnPoint.position
            : new Vector3(3f, 0f, 0f);

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.SetSpeedMultiplier(speedMultiplier);
            enemyBase.SetPlayerParrySystem(playerTransform.GetComponent<ParrySystem>());
        }

        enemiesAlive++;
        Debug.Log($"Spawned {prefab.name} — enemies alive: {enemiesAlive}");
    }

    public void ReportEnemyDead()
    {
        enemiesAlive--;
        Debug.Log($"Enemy died — enemies alive: {enemiesAlive}");

        if (enemiesAlive <= 0 && !isSpawning)
        {
            onWaveCompleted.Invoke(currentWaveIndex + 1);
            currentWaveIndex++;
            StartCoroutine(StartWave(currentWaveIndex));
        }
    }
}