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

    // Added Features
    private bool _gameStarted = false;
    private float spawnInterval = 2.0f; // initial spawn interval
    private float speedIncreaseInterval = 10.0f; 
    private float obstacleSpeed = 0.1f; // initial obstacle speed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Ensures that the Singleton instance retains between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game has started
        if (_gameStarted)
        {
            ground.transform.Translate(Vector3.down * Time.deltaTime * obstacleSpeed, Space.World);

            if (ground.transform.position.y <= -10)
            {
              Destroy(ground);
            }
        }
    }

    public void StartLevel()
    {
        _gameStarted = true;
        StartCoroutine(SpawnObstacles());
        StartCoroutine(IncreaseObstacleSpeed());
    }

    private IEnumerator SpawnObstacles() 
    {
        while (_gameStarted)
        {
            var obstacleClone = Instantiate(obstacleWide, GetRandomSpawnPosition());
            obstacleClone.AddComponent<ObstacleMovement>(); // Assuming the ObstacleMovement script exists and implemented correctly
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