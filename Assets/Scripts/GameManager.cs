using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // static instance of GameManager which allows it to be accessed by any other script 
    public static GameManager instance;
    
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerManager.Instance.PlayerDied.AddListener(OnPlayerDied);    
    }

    private void OnPlayerDied()
    {
        Debug.Log("Resetting Game");
        Invoke(nameof(ResetGame), 3f);
    }

    private void ResetGame()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reloads the current active scene
        SceneManager.LoadScene(currentScene.name);
    }
}