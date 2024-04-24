using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // static instance of GameManager which allows it to be accessed by any other script 
    public static GameManager instance;
    public TextMeshProUGUI ScoreText;
    private float score;
    private bool isPlayerAlive = false;

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
        score = 0;
        ScoreText.text = "Score: " + Mathf.Round(score).ToString();
    }

    private void FixedUpdate()
    {
        if (isPlayerAlive) 
        {
            score += Time.fixedDeltaTime;
            ScoreText.text = "Score: " + Mathf.Round(score).ToString();
        }
    }

    public void StartTheScore()
    {
        isPlayerAlive = true;
    }

    private void OnPlayerDied()
    {
        isPlayerAlive = false;
        ScoreText.color = Color.red;
        ScoreText.fontSize += 10; // increase size by 10
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