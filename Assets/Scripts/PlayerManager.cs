using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }  // Singleton instance

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI text;
    
    public UnityEvent PlayerDied;

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
        canvasGroup.alpha = 1;
        SetMainUIText("Press F to Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if 'F' is pressed and if text equals "Press F to Spawn"
        if (Input.GetKeyDown(KeyCode.F) && text.text.Equals("Press F to Spawn"))
        {
            StartCoroutine(SpawnPlayer());
        }
    }

    IEnumerator SpawnPlayer()
    {
        Debug.Log("Instantiating Player...");
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        // StartCoroutine(FadeCanvasGroup(0, 1f)); // Smoothly hide UI
        yield return new WaitForSeconds(1); // Wait for a moment when player is instantiated
        StartCoroutine(PerformCountDown(3));
    }
    
    public IEnumerator FadeCanvasGroup(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Ensure the Fade is completed
    }
    
    IEnumerator PerformCountDown(int count)
    {
        while (count > 0)
        {
            yield return new WaitForSeconds(1f);
            SetMainUIText(count.ToString());
            count--;
        }

        text.text = "CLIMB";
        LevelManager.Instance.StartLevel();
            
        yield return new WaitForSeconds(1f);

        GameManager.instance.StartTheScore();
        StartCoroutine(FadeCanvasGroup(0, 1f)); // Smoothly hide UI Again after countdown
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetMainUIText("Wasted");
            StartCoroutine(FadeCanvasGroup(1, 1f)); // Smoothly hide UI Again after countdown
            PlayerDied?.Invoke();
        }
    }

    public void SetMainUIText(string inputText)
    {
        if (!string.IsNullOrEmpty(inputText))
        {
            text.text = inputText;
        }
    }
}