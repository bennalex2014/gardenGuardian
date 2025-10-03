using UnityEngine;
using System.Collections;

public class CrowSpawnerScript : MonoBehaviour
{
    [Header("Crow Prefabs")]
    public GameObject basicCrowPrefab;
    public GameObject fastCrowPrefab;
    public GameObject strongCrowPrefab;
    public GameObject turboCrowPrefab;

    public Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Spawns a specific crow type by name
    /// </summary>
    public void SpawnCrow(string crowType)
    {
        GameObject prefabToSpawn = null;

        switch (crowType)
        {
            case "BasicCrow":
                prefabToSpawn = basicCrowPrefab;
                break;
            case "FastCrow":
                prefabToSpawn = fastCrowPrefab;
                break;
            case "StrongCrow":
                prefabToSpawn = strongCrowPrefab;
                break;
            case "TurboCrow":
                prefabToSpawn = turboCrowPrefab;
                break;
            default:
                Debug.LogError($"Unknown crow type: {crowType}");
                return;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError($"Prefab not assigned for {crowType}!");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Gets a random spawn position at the edge of camera bounds
    /// </summary>
    Vector3 GetRandomSpawnPosition()
    {
        // Get camera bounds in world space
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float verticalBound = cameraHeight / 2f;
        float horizontalBound = cameraWidth / 2f;

        Vector3 spawnPosition;

        // 30% chance to spawn from top/bottom, 70% from sides
        if (Random.value > 0.7f)
        {
            // Spawn from top or bottom
            float x = Random.Range(-horizontalBound, horizontalBound);
            float y = Random.value > 0.5f ? verticalBound : -verticalBound;
            spawnPosition = new Vector3(x, y, 0);
        }
        else
        {
            // Spawn from left or right side
            float x = Random.value > 0.5f ? horizontalBound : -horizontalBound;
            float y = Random.Range(-verticalBound, verticalBound);
            spawnPosition = new Vector3(x, y, 0);
        }

        return spawnPosition;
    }

    // Legacy method - keeping for backwards compatibility if needed
    public void spawnBasicCrows(int numberOfCrows, float timePeriod)
    {
        StartCoroutine(SpawnCrowsOverTime(numberOfCrows, timePeriod));
    }

    private IEnumerator SpawnCrowsOverTime(int numberOfCrows, float timePeriod)
    {
        float delayBetweenSpawns = timePeriod / numberOfCrows;

        for (int i = 0; i < numberOfCrows; i++)
        {
            SpawnCrow("BasicCrow");
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}