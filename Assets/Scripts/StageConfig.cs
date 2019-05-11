using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stage Config")]
public class StageConfig : ScriptableObject {
    [SerializeField] List<WaveConfig> normalWaveConfigs;
    [SerializeField] int numberOfUpgradeItems = 3;
    [SerializeField] int numberOfHealingItems = 2;
    [SerializeField] WaveConfig[] bossWaveConfigs;
    [Header("Stage Behavior")]
    [SerializeField] bool randomWaves = false;
    [SerializeField] bool randomBosses = false;

    public bool RandomWaves
    {
        get
        {
            return randomWaves;
        }
    }

    public bool RandomBosses
    {
        get
        {
            return randomBosses;
        }
    }

    public List<WaveConfig> NormalWaveConfigs
    {
        get
        {
            return normalWaveConfigs;
        }
    }

    public int NumberOfUpgradeItems
    {
        get
        {
            return numberOfUpgradeItems;
        }
    }

    public int NumberOfHealingItems
    {
        get
        {
            return numberOfHealingItems;
        }
    }
}
