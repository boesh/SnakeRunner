using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private List<GameObject> pool;

    public Pool()
    {
        this.pool = new List<GameObject>();
    }

    public void AddObjectToPool(GameObject prefab)
    {
        pool.Add(GameObject.Instantiate(prefab));
    }

    public GameObject GetObjectFromPool(GameObject prefab)
    {
        for (int i = 0; i < pool.Count; ++i)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        
        AddObjectToPool(prefab);
        pool[pool.Count - 1].SetActive(true);
        return pool[pool.Count - 1];
    }
}
