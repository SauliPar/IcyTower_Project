using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FinishPlatformHandler : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y >= 0)
        {
            transform.Translate(Vector3.down * Time.deltaTime * LevelManager.Instance.ObstacleSpeed, Space.World);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.LevelCompleted();
            PlayerManager.Instance.SetMainUIText("You beat the level! Loistoa :DDd");
            PlayerManager.Instance.FadeCanvasGroup(1, .5f);
        }
    }
}
