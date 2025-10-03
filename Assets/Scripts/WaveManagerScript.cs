using System;
using UnityEngine;
using System.Collections;

public class WaveManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int basicCrowCount;
        public int fastCrowCount;
        public int strongCrowCount;
        public int turboCrowCount;
        public float spawnDuration; // How long to spawn all crows over
        public int goldReward;
    }

    [Header("Wave Configuration")]
    public Wave[] waves;

    [Header("Wave Timing")]
    public float delayBetweenWaves = 8f;
    public int startingGold = 20;

    [Header("Current State")]
    public int currentWaveIndex = 0;
    public int activeCrowCount = 0;
    public bool waveInProgress = false;
    public bool spawningComplete = false;
    public bool allWavesComplete = false;

    private CrowSpawnerScript crowSpawner;
    private PlayerScript player;

    void Start()
    {
        crowSpawner = FindFirstObjectByType<CrowSpawnerScript>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        // Initialize default waves if none set
        if (waves == null || waves.Length == 0)
        {
            InitializeDefaultWaves();
        }

        // Give starting gold
        player.gold = startingGold;

        // Start first wave after short delay
        StartCoroutine(StartWaveAfterDelay(2f));
    }

    void InitializeDefaultWaves()
    {
        waves = new Wave[3];

        // Wave 1 - Tutorial difficulty
        waves[0] = new Wave
        {
            basicCrowCount = 8,
            fastCrowCount = 0,
            strongCrowCount = 0,
            turboCrowCount = 0,
            spawnDuration = 15f,
            goldReward = 0
        };

        // Wave 2 - Introduce variety
        waves[1] = new Wave
        {
            basicCrowCount = 10,
            fastCrowCount = 5,
            strongCrowCount = 0,
            turboCrowCount = 0,
            spawnDuration = 20f,
            goldReward = 15
        };

        // Wave 3 - Final challenge
        waves[2] = new Wave
        {
            basicCrowCount = 10,
            fastCrowCount = 8,
            strongCrowCount = 5,
            turboCrowCount = 2,
            spawnDuration = 30f,
            goldReward = 20
        };

        Debug.Log("WaveManager: Initialized 3 default waves");
    }

    IEnumerator StartWaveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextWave();
    }

    void StartNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            // All waves complete - player wins!
            allWavesComplete = true;
            Debug.Log("ALL WAVES COMPLETE - PLAYER WINS!");
            // TODO: Trigger win screen via GameSceneManager
            return;
        }

        Wave currentWave = waves[currentWaveIndex];
        waveInProgress = true;
        spawningComplete = false;

        // Give gold reward for this wave
        player.gold += currentWave.goldReward;

        Debug.Log($"Starting Wave {currentWaveIndex + 1}! Gold reward: {currentWave.goldReward}");

        // Start spawning crows
        StartCoroutine(SpawnWave(currentWave));
    }

    IEnumerator SpawnWave(Wave wave)
    {
        // Calculate total crows for this wave
        int totalCrows = wave.basicCrowCount + wave.fastCrowCount + 
                        wave.strongCrowCount + wave.turboCrowCount;

        if (totalCrows == 0)
        {
            Debug.LogWarning("Wave has 0 crows!");
            spawningComplete = true;
            CheckWaveComplete();
            yield break;
        }

        float delayBetweenSpawns = wave.spawnDuration / totalCrows;

        // Spawn BasicCrows
        for (int i = 0; i < wave.basicCrowCount; i++)
        {
            crowSpawner.SpawnCrow("BasicCrow");
            OnCrowSpawned();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        // Spawn FastCrows
        for (int i = 0; i < wave.fastCrowCount; i++)
        {
            crowSpawner.SpawnCrow("FastCrow");
            OnCrowSpawned();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        // Spawn StrongCrows
        for (int i = 0; i < wave.strongCrowCount; i++)
        {
            crowSpawner.SpawnCrow("StrongCrow");
            OnCrowSpawned();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        // Spawn TurboCrows
        for (int i = 0; i < wave.turboCrowCount; i++)
        {
            crowSpawner.SpawnCrow("TurboCrow");
            OnCrowSpawned();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        spawningComplete = true;
        Debug.Log($"Wave {currentWaveIndex + 1} spawning complete. Waiting for crows to be defeated...");
        CheckWaveComplete();
    }

    public void OnCrowSpawned()
    {
        activeCrowCount++;
    }

    public void OnCrowDied()
    {
        activeCrowCount--;
        CheckWaveComplete();
    }

    void CheckWaveComplete()
    {
        // Wave is complete when spawning is done AND all crows are dead
        if (spawningComplete && activeCrowCount <= 0 && waveInProgress)
        {
            waveInProgress = false;
            currentWaveIndex++;

            Debug.Log($"Wave {currentWaveIndex} complete!");

            if (currentWaveIndex < waves.Length)
            {
                // Start next wave after delay
                StartCoroutine(StartWaveAfterDelay(delayBetweenWaves));
            }
            else
            {
                // All waves done
                allWavesComplete = true;
                Debug.Log("ALL WAVES COMPLETE - PLAYER WINS!");
                // TODO: Trigger win screen
            }
        }
    }
}