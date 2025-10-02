using System;
using UnityEngine;

public class WaveManagerScript : MonoBehaviour
{

    public Boolean waveActive = true;

    private CrowSpawnerScript crowSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crowSpawner = FindFirstObjectByType<CrowSpawnerScript>();
        if (waveActive)
        {
            startWave1();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void startWave1()
    {
        crowSpawner.spawnBasicCrows(5, 10f);
    }
}
