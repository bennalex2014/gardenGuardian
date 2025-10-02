using System;
using UnityEngine;

public class WaveManagerScript : MonoBehaviour
{

    public Boolean waveActive = true;
    private PlayerScript player;


    private CrowSpawnerScript crowSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crowSpawner = FindFirstObjectByType<CrowSpawnerScript>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        
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
        player.gold += 20;
        crowSpawner.spawnBasicCrows(5, 10f);
    }
}
