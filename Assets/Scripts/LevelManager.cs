using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }  // Singleton instance
    public float ObstacleSpeed = 1f;

    [SerializeField] private GameObject ground;
    [SerializeField] private List<Transform> obstacleSpawnTransforms;
    [SerializeField] private GameObject obstacleWide;

    private bool _gameStarted = false;
    private float spawnInterval = 2.0f; // initial spawn interval
    private float speedIncreaseInterval = 10.0f; 
    private float obstacleSpeed = 0.1f; // initial obstacle speed
    
    private float _gameStartTimestamp;
    [SerializeField] private GameObject obstaclePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Ensures that the singleton instance retains between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (_gameStarted)
        {
            // Don't start moving the ground until 4 seconds into the game
            if (Time.time >= _gameStartTimestamp + 4f)
            {
                if (ground != null)
                {
                    ground.transform.Translate(Vector3.down * Time.deltaTime * obstacleSpeed, Space.World);
                    
                    if (ground.transform.position.y <= -10)
                    {
                        ObjectPoolManager.Instance.Release(ground);  // Release ground object back into the pool
                    }
                } 
            }
        }
    }

    public void StartLevel()
    {
        _gameStarted = true;
        _gameStartTimestamp = Time.time; // Save the time when the game started
        StartCoroutine(SpawnObstacles());
        StartCoroutine(IncreaseObstacleSpeed());
    }

    private IEnumerator SpawnObstacles() 
    {
        while (_gameStarted)
        {
            var poppedObstacle = ObjectPoolManager.Instance.Get(obstaclePrefab);
        
            if (poppedObstacle != null)
            {
                poppedObstacle.transform.position = GetRandomSpawnPosition().position;  // Set the position of the obstacle
                poppedObstacle.AddComponent<ObstacleMovement>();
            }
        
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Transform GetRandomSpawnPosition()
    {
        var randomIndex = Random.Range(0, obstacleSpawnTransforms.Count);
        return obstacleSpawnTransforms[randomIndex];
    }

    private IEnumerator IncreaseObstacleSpeed()
    {
        while (_gameStarted)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);
            obstacleSpeed += 0.1f;
            spawnInterval = Mathf.Max(spawnInterval - 0.1f, 0.1f); // subtract 0.1 from spawnInterval every speedIncreaseInterval, but don't let spawnInterval go below 0.1
        }
    }
}