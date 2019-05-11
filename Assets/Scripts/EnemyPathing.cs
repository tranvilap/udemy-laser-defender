using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Vector3> waypoints;
    int waypointIndex = 0;
    bool destroyAtEndPoint;
    bool stopAtEndPoint = false;
    bool isAimingPlayer = false;
    public bool DestroyAtEndPoint
    {
        set
        {
            destroyAtEndPoint = value;
        }
    }

    public bool StopAtEndPoint
    {
        set
        {
            stopAtEndPoint = value;
        }
    }

    public WaveConfig WaveConfig
    {
        set
        {
            waveConfig = value;
        }
    }

    public List<Vector3> Waypoints
    {
        set
        {
            waypoints = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (waveConfig.ChargeToPlayer)
        {
            StartCoroutine(SetChargeTarget());
        }
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex];
            var movementThisFrame = waveConfig.MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == waypoints[waypointIndex])
            {
                waypointIndex++;
            }
        }
        else
        {
            if (destroyAtEndPoint)
            {
                if (waveConfig.ChargeToPlayer)
                {
                    if (isAimingPlayer)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else if (stopAtEndPoint) { }
            else
            {
                waypointIndex = 0;
            }

            
        }
    }
    IEnumerator SetChargeTarget()
    {
        yield return new WaitForSeconds(2f);
        if (isAimingPlayer == false)
        {

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                
                Vector2 playerPos = player.gameObject.transform.position;
                Vector2 tempPos = (Vector2)this.gameObject.transform.position - playerPos;
                Vector2 targetPos = playerPos - tempPos;
                Camera mainCamera = Camera.main;
                while
                    (
                        targetPos.x > mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x
                        && targetPos.x < mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x
                        && targetPos.y > mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y
                        && targetPos.y < mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y
                    )
                {
                    tempPos = playerPos - targetPos;
                    targetPos = targetPos - tempPos;
                }
                waypoints.Add(targetPos);
            }
            isAimingPlayer = true;
        }
    }
}
