using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }  // Singleton instance

    [SerializeField] private GameObject ground;
    [SerializeField] private List<Transform> obstacleSpawnTransforms;
    [SerializeField] private GameObject obstacleWide;

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

    }
}