using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    none,
    tile,
}

[System.Serializable]
public class PoolItem
{
    public PoolType poolType;
    public GameObject prefab;
    public int initialSize;
}

public class Pool : Singleton<Pool>
{
    [SerializeField] private List<PoolItem> _poolItems = new List<PoolItem>();

    Dictionary<PoolType, Queue<GameObject>> _poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

    void Awake()
    {
        foreach (PoolItem poolItem in _poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < poolItem.initialSize; i++)
            {
                GameObject obj = Instantiate(poolItem.prefab);
                obj.SetActive(false);
                obj.transform.parent = transform;
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(poolItem.poolType, objectPool);
        }
    }

    public GameObject GetPooledObject(PoolType poolType)
    {
        if (_poolDictionary.ContainsKey(poolType) && _poolDictionary[poolType].Count > 0)
        {
            GameObject obj = _poolDictionary[poolType].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = transform;

        PoolType poolType = PoolType.none;
        foreach (PoolItem poolItem in _poolItems)
        {
            if (obj.name.Contains(poolItem.prefab.name))
            {
                poolType = poolItem.poolType;
                break;
            }
        }

        if (poolType != PoolType.none)
        {
            _poolDictionary[poolType].Enqueue(obj);
        }
    }
}
