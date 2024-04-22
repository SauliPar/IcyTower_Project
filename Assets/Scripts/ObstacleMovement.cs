using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * LevelManager.Instance.ObstacleSpeed, Space.World);

        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }
}