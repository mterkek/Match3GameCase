using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Basit prefab havuzu. Tipik olarak Tile prefab’ları için kullanılır.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private int initialSize = 30;

        private readonly Queue<GameObject> pool = new Queue<GameObject>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            for (int i = 0; i < initialSize; i++)
                pool.Enqueue(CreateNew());
        }

        private GameObject CreateNew()
        {
            var go = Instantiate(tilePrefab);
            go.SetActive(false);
            return go;
        }

        public GameObject Get()
        {
            if (pool.Count == 0) pool.Enqueue(CreateNew());
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}
