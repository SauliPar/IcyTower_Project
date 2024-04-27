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
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject endPlatform;

    private bool _gameStarted = false;
    private float spawnInterval = 2f; // initial spawn interval
    private float speedIncreaseInterval = 5.0f; 
    // private float obstacleSpeed = 0.2f; // initial obstacle speed
    
    private float _gameStartTimestamp;
    private bool _gameIsEnding;

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
            if (Time.time >= _gameStartTimestamp + 8f)
            {
                if (ground != null)
                {
                    ground.transform.Translate(Vector3.down * Time.deltaTime * .2f, Space.World);
                    
                    if (ground.transform.position.y <= -8)
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
        GameManager.instance.StartEndGame.AddListener(HandleEndGame);    
    }

    private void HandleEndGame()
    {
        _gameIsEnding = true;
    }

    private IEnumerator SpawnObstacles() 
    {
        while (_gameStarted)
        {
            var poppedObstacle = ObjectPoolManager.Instance.Get(obstaclePrefab);
            
            if (_gameIsEnding)
            {
                poppedObstacle = ObjectPoolManager.Instance.Get(endPlatform);
            }
        
            if (poppedObstacle != null)
            {
                poppedObstacle.transform.position = GetRandomSpawnPosition().position;  // Set the position of the obstacle
                // poppedObstacle.AddComponent<ObstacleMovement>();
            }

            if (!_gameIsEnding)
            {
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(50);
            }
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
            if (ObstacleSpeed <= 4.0f) 
            {
                ObstacleSpeed += .2f;  // Increase ObstacleSpeed by 0.2 every interval until it reaches a maximum value of 4.0
            }
            
            // spawnInterval = Mathf.Min(spawnInterval + 0.1f, 3.0f);        
        }
    }
}