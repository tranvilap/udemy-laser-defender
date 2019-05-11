using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {
    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] pathPrefabs;
    [Header("Wave properties")]
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float randomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;
    [Header("Wave Behaviors")]
    [SerializeField] bool destroyAtEndPoint = false;
    [SerializeField] bool isAbleToShoot = true;
    [SerializeField] bool stopAtEndPoint = false;
    [SerializeField] bool deployAtSameTime = false;
    [SerializeField] bool chargeToPlayer = false;

    public GameObject EnemyPrefab
    {
        get
        {
            return enemyPrefab;
        }
    }

    public float TimeBetweenSpawns
    {
        get
        {
            return timeBetweenSpawns;
        }
    }

    public float RandomFactor
    {
        get
        {
            return randomFactor;
        }
    }

    public int NumberOfEnemies
    {
        get
        {
            return numberOfEnemies;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public List<List<Vector3>> Waypoints //Get all waypoints in each Path Prefab - return a list contains waypoint lists
    {
        get
        {
            List<List<Vector3>> waypoints = new List<List<Vector3>>();
            foreach(GameObject item in pathPrefabs)
            {
                List<Vector3> transformList = new List<Vector3>();
                foreach(Transform transform in item.transform)
                {
                    transformList.Add(transform.position);
                }
                waypoints.Add(transformList);
            }
            return waypoints;
        }
    }

    public bool DestroyAtEndPoint
    {
        get
        {
            return destroyAtEndPoint;
        }
    }

    public bool IsAbleToShoot
    {
        get
        {
            return isAbleToShoot;
        }

    }

    public bool StopAtEndPoint
    {
        get
        {
            return stopAtEndPoint;
        }
    }

    public GameObject[] PathPrefabs
    {
        get
        {
            return pathPrefabs;
        }
    }

    public bool DeployAtSameTime
    {
        get
        {
            return deployAtSameTime;
        }
    }

    public bool ChargeToPlayer
    {
        get
        {
            return chargeToPlayer;
        }
    }
}
