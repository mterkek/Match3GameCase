using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Grid
{
    /// Boş hücreleri dolduran ve tile spawn animasyonlarını yöneten sınıf.
    /// Grid oluşturulduğunda ve boş hücreler oluştuğunda tetiklenir.
    public class TileSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridSystem gridSystem;

        [Header("Tile Settings")]
        [SerializeField] private Sprite[] tileTypeSprites;
        [SerializeField] private float spawnHeight = 20f;
        [SerializeField] private float dropDuration = 0.3f;
        [SerializeField] private float spawnDelayBetweenTiles = 0.05f;

        private int _pendingSpawns;

        private void Awake()
        {
            if (gridSystem == null)
                gridSystem = GridSystem.Instance;
        }

        private void OnEnable()
        {
            EventSystem.ActionGridCreated += FillInAllTheEmptyCells;
            EventSystem.ActionFillInEmptyCells += FillInAllTheEmptyCells;
            EventSystem.ActionRefill += FillInAllTheEmptyCells;
        }

        private void OnDisable()
        {
            EventSystem.ActionGridCreated -= FillInAllTheEmptyCells;
            EventSystem.ActionFillInEmptyCells -= FillInAllTheEmptyCells;
            EventSystem.ActionRefill -= FillInAllTheEmptyCells;
        }

        private void FillInAllTheEmptyCells()
        {
            var emptyCells = new List<KeyValuePair<Vector2Int, GridSystem.GridCell>>();
            KeyValuePair<Vector2Int, GridSystem.GridCell>? emptyCell;

            // Boş hücreleri topla (aynı hücreyi birden fazla kez kullanmamak için).
            while ((emptyCell = gridSystem.GetEmptyGridCell()) != null)
            {
                emptyCells.Add(emptyCell.Value);
                // Geçici olarak boş bırakılan hücreyi işaretle
                // (şimdilik null bırakılıyor, spawn sırasında dolacak).
                // Bu döngü, içeriğin aynı hücrede takılı kalmasını önler.
                gridSystem.SetTileToCell(emptyCell.Value.Key, null);
            }

            if (emptyCells.Count == 0)
            {
                EventSystem.ActionSpawnCompleted?.Invoke();
                return;
            }

            _pendingSpawns = emptyCells.Count;

            for (int i = 0; i < emptyCells.Count; i++)
            {
                float delay = i * spawnDelayBetweenTiles;
                SpawnTile(emptyCells[i], delay);
            }
        }

        private void SpawnTile(KeyValuePair<Vector2Int, GridSystem.GridCell> emptyCell, float delay = 0f)
        {
            GameObject pooled = Pool.Instance.GetPooledObject(PoolType.tile);
            Tile tile = pooled.GetComponent<Tile>();

            ETileType type = GetRandomTileType();
            Sprite sprite = GetSpriteForType(type);

            tile.Init(type, emptyCell.Key, sprite);
            emptyCell.Value.currentTile = tile;

            Vector3 gridPosition = emptyCell.Value.cellObject.transform.position;
            var spawnPosition = new Vector3(gridPosition.x, spawnHeight, 0);
            pooled.transform.position = spawnPosition;

            DOVirtual.DelayedCall(delay, () =>
            {
                pooled.transform.DOMove(gridPosition, dropDuration)
                    .SetEase(Ease.InBack, .5f)
                    .OnComplete(OnSpawnTweenComplete);
            });
        }

        private void OnSpawnTweenComplete()
        {
            _pendingSpawns--;
            if (_pendingSpawns <= 0)
            {
                EventSystem.ActionSpawnCompleted?.Invoke();
            }
        }

        private ETileType GetRandomTileType()
        {
            Array values = Enum.GetValues(typeof(ETileType));
            return (ETileType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        private Sprite GetSpriteForType(ETileType type)
        {
            var index = (int)type;
            if (tileTypeSprites != null && index >= 0 && index < tileTypeSprites.Length)
                return tileTypeSprites[index];

            return null;
        }
    }
}
