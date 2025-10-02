using UnityEngine;
using System.Collections;

public class CrowSpawnerScript : MonoBehaviour
{
    public GameObject basicCrowPrefab;
    public Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // spawns a crow at a random position given if it spawning at the side of the map or at the top/bottom.
    // should spawn a crow from a prefab named "BasicCrow" located in a prefabs folder within assets.
    void spawnBasicCrow()
    {
        // Get camera bounds in world space
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float verticalBound = cameraHeight / 2f;
        float horizontalBound = cameraWidth / 2f;

        Vector3 spawnPosition;

        // Randomly choose to spawn from side or top/bottom
        if (Random.value > 0.7f)
        {
            // Spawn from left or right side
            float x = Random.value > 0.5f ? horizontalBound : -horizontalBound;
            float y = Random.Range(-verticalBound, verticalBound);
            spawnPosition = new Vector3(x, y, 0);
        }
        else
        {
            // Spawn from top or bottom
            float x = Random.Range(-horizontalBound, horizontalBound);
            float y = Random.value > 0.5f ? verticalBound : -verticalBound;
            spawnPosition = new Vector3(x, y, 0);
        }

        Instantiate(basicCrowPrefab, spawnPosition, Quaternion.identity);
    }

    // calls spawnCrow() multiple times to spawn a set number of crows across a period of time.
    // parameters: int numberOfCrows, float timePeriod
    public void spawnBasicCrows(int numberOfCrows, float timePeriod)
    {
        StartCoroutine(SpawnCrowsOverTime(numberOfCrows, timePeriod));
    }

    private IEnumerator SpawnCrowsOverTime(int numberOfCrows, float timePeriod)
    {
        float delayBetweenSpawns = timePeriod / numberOfCrows;

        for (int i = 0; i < numberOfCrows; i++)
        {
            spawnBasicCrow();
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}
