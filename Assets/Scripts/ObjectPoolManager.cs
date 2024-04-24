using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private int capacity;

    private Dictionary<GameObject, Queue<GameObject>> pool;
    
    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pool = new Dictionary<GameObject, Queue<GameObject>>();
    }

    public GameObject Get(GameObject prefab)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
            FillPoolWithPrefab(prefab, pool[prefab]);
        }

        if (pool[prefab].Count > 0) 
        {
            var obj = pool[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = Instantiate(prefab);
            obj.SetActive(true);
            return obj;
        }
    }

    public void Release(GameObject obj)
    {
        foreach (var prefab in pool.Keys)
        {
            if (obj.Equals(prefab))
            {
                obj.SetActive(false);
                pool[prefab].Enqueue(obj);
                return;
            }
        }
        Destroy(obj);  // The object is not from any pool. Destroy it.
    }

    private void FillPoolWithPrefab(GameObject prefab, Queue<GameObject> queue)
    {
        for (int i = 0; i < capacity; i++)
        {
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }
}