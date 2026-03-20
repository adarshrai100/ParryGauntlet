using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ParryGauntlet/Wave Config")]
public class WaveConfigSO : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public float spawnDelay = 0f;
    }

    [Header("Wave Settings")]
    public string waveName = "Wave 1";
    public float timeBetweenSpawns = 1.5f;
    public float waveStartDelay = 2f;

    [Header("Enemies")]
    public List<EnemySpawnData> enemies = new List<EnemySpawnData>();

    [Header("Difficulty")]
    [Range(0.5f, 2f)]
    public float speedMultiplier = 1f;
}