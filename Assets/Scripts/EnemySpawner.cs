using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<StageConfig> stageConfigs;
    [Header("Parameters")]
    [SerializeField] int startingWave = 0;
    [SerializeField] float timeBetweenWaves = 1f;
    [Header("Spawning Behaviors")]
    [SerializeField] bool allStagesLooping = false;
    [SerializeField] bool randomStages = false;

    // Use this for initialization
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllStages());
        }
        while (allStagesLooping);
    }



    private IEnumerator SpawnAllEnemiesInPath(WaveConfig wave, int position, int numberUpgradeItem)
    {
        int numberOfEnemies = wave.NumberOfEnemies;
        List<bool> itemStatus = new List<bool>();
        List<int> enemyIndexes = new List<int>();
        if(numberUpgradeItem < numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                itemStatus.Add(false);
                enemyIndexes.Add(i);
            }
            for (int i = 0; i < numberUpgradeItem; i++)
            {
                int randPos = Random.Range(0, enemyIndexes.Count);
                itemStatus[randPos] = true;
                enemyIndexes.RemoveAt(randPos);
            }
        }
        else
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                itemStatus.Add(true);
            }
        }
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject enemy = Instantiate(wave.EnemyPrefab, wave.Waypoints[position][0], Quaternion.identity);
            enemy.GetComponent<Enemy>().IsAbleToShoot = wave.IsAbleToShoot;
            enemy.GetComponent<Enemy>().HasItem = itemStatus[i];
            enemy.GetComponent<EnemyPathing>().DestroyAtEndPoint = wave.DestroyAtEndPoint;
            enemy.GetComponent<EnemyPathing>().StopAtEndPoint = wave.StopAtEndPoint;
            enemy.GetComponent<EnemyPathing>().WaveConfig = wave;
            enemy.GetComponent<EnemyPathing>().Waypoints = wave.Waypoints[position];
            yield return new WaitForSeconds(wave.TimeBetweenSpawns);
        }
    }

    private IEnumerator SpawnAllPathsInWave(WaveConfig wave, int numberUpgradeItems)
    {
        //Chia so luong item cho tung path, sau do chia so luong item trong tung path cho enemy
        int[] numberItemForEachPath = new int[wave.PathPrefabs.Length];
        for (int i = 0; i < wave.PathPrefabs.Length; i++)
        {
            int randomNumberOfItem = Random.Range(0, numberUpgradeItems + 1);
            if (i < wave.PathPrefabs.Length - 1)
            {
                numberItemForEachPath[i] = randomNumberOfItem;
                numberUpgradeItems -= randomNumberOfItem;
            }
            else
            {
                numberItemForEachPath[i] = numberUpgradeItems;
            }
            if (!wave.DeployAtSameTime)
            {
                yield return StartCoroutine(SpawnAllEnemiesInPath(wave, i, numberItemForEachPath[i]));
            }
            else
            {
                StartCoroutine(SpawnAllEnemiesInPath(wave, i, numberItemForEachPath[i]));
            }
        }
        yield return new WaitForSeconds(timeBetweenWaves);

    }

    private IEnumerator SpawnAllWavesInStage(StageConfig stage)
    {
        List<WaveConfig> waveConfigs = stage.NormalWaveConfigs;
        bool randomWaves = stage.RandomWaves;
        int numberOfUpgradeItems = stage.NumberOfUpgradeItems;
        //int numberOfHealingItems = stage.NumberOfHealingItems;
        int[] numberOfItemsInEachWave = new int[waveConfigs.Count];

        // START ADDING NUMBER OF ITEMS INTO WAVES
        for (int i = 0; i < waveConfigs.Count - 1; i++)
        {
            int randomNumberItemInAWave = Random.Range(0, numberOfUpgradeItems + 1);
            numberOfItemsInEachWave[i] = randomNumberItemInAWave;
            numberOfUpgradeItems -= randomNumberItemInAWave;
            Debug.Log("Number of item in wave " + i + ": " + numberOfItemsInEachWave[i]);
        }
        Debug.Log("Number of item in wave " + (waveConfigs.Count - 1) + ": " + numberOfItemsInEachWave[waveConfigs.Count - 1]);
        numberOfItemsInEachWave[waveConfigs.Count - 1] = numberOfUpgradeItems;
        // END ADDING NUMBER OF ITEMS INTO WAVES

        if (!randomWaves)
        {
            for (int i = startingWave; i < waveConfigs.Count; i++)
            {
                yield return StartCoroutine(SpawnAllPathsInWave(waveConfigs[i], numberOfItemsInEachWave[i]));
            }
        }
        else
        {
            List<int> randList = new List<int>();
            for (int i = startingWave; i < waveConfigs.Count; i++)
            {
                randList.Add(i);
            }
            for (int i = startingWave; i < waveConfigs.Count; i++)
            {
                int randPos = Random.Range(0, randList.Count);
                int randValue = randList[randPos];
                randList.RemoveAt(randPos);
                yield return StartCoroutine(SpawnAllPathsInWave(waveConfigs[randValue], numberOfItemsInEachWave[randValue]));
            }
        }
    }

    private IEnumerator SpawnAllStages()
    {
        if (!randomStages)
        {
            for (int i = 0; i < stageConfigs.Count; i++)
            {
                yield return StartCoroutine(SpawnAllWavesInStage(stageConfigs[i]));
            }
        }
    }

}
