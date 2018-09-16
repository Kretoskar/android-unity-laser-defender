using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private List<WaveConfig> waveConfigs;

    private int startingWave = 0;

	// Use this for initialization
	void Start () {
        var currentWave = waveConfigs[startingWave];
        StartCoroutine(SpawnAllWaves());
	}

    private IEnumerator SpawnAllWaves()
    {
        while (true)
        {
            int allWaves = waveConfigs.Count;
            int currentWaveIndex = UnityEngine.Random.Range(0, allWaves);
            var currentWave = waveConfigs[currentWaveIndex];
            StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            float timer = UnityEngine.Random.Range(1.5f, 3f);
            yield return new WaitForSeconds(timer);
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
