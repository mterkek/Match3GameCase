using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Systems
{
    using Grid;
    using Core;

    /// <summary>
    /// Boş hücrelere yukarıdan yeni tile düşürür.
    /// </summary>
    public class TileSpawner : MonoBehaviour
    {
        public static TileSpawner Instance { get; private set; }

        [SerializeField] private float spawnHeight = 10f;
        [SerializeField] private float dropTime = 0.3f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            EventManager.OnMergeComplete += SpawnForEmptyCells;
        }

        private void OnDestroy()
        {
            EventManager.OnMergeComplete -= SpawnForEmptyCells;
        }

        public void SpawnForEmptyCells()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            var empties = new List<BoardManager.GridCell>();
            foreach (var cell in BoardManager.Instance.AllCells)
                if (cell.tile == null) empties.Add(cell);

            foreach (var cell in empties)
            {
                var go = ObjectPool.Instance.Get();
                var controller = go.GetComponent<TileController>();
                var type = (ETileType)Random.Range(0, System.Enum.GetValues(typeof(ETileType)).Length);
                controller.Initialize(type, cell.gridPos);
                go.transform.position = cell.worldPosition + Vector3.up * spawnHeight;
                cell.tile = controller;
                cell.tileType = type;

                go.transform.DOMove(cell.worldPosition, dropTime);
            }

            yield return new WaitForSeconds(dropTime + 0.05f);
            EventManager.RaiseSpawnComplete();
        }
    }
}
