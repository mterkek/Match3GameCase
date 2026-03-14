using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Grid sistemi, ızgarayı yönetir ve hücre işlemlerini sağlar.
    /// </summary>
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance { get; private set; }

        [Serializable]
        public class GridCell
        {
            public GameObject cellObject;
            public Tile currentTile;
            public Vector2Int position;
        }

        [Header("Grid Settings")]
        [SerializeField] private Vector2Int gridSize = new Vector2Int(6, 6);
        [SerializeField] private float cellSpacing = 1f;
        [SerializeField] private GameObject gridCellPrefab;

        private readonly Dictionary<Vector2Int, GridCell> _dictionaryGridCells = new Dictionary<Vector2Int, GridCell>();

        public Vector2Int GridSize => gridSize;

        /// <summary>
        /// Dışarıdan grid hücrelerine erişim sağlar.
        /// </summary>
        public IReadOnlyDictionary<Vector2Int, GridCell> Cells => _dictionaryGridCells;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            CreateGrid(gridSize);
        }

        private void CreateGrid(Vector2Int size)
        {
            // Eğer daha önce bir grid oluşturulduysa önce temizle
            foreach (KeyValuePair<Vector2Int, GridCell> kv in _dictionaryGridCells)
            {
                if (kv.Value.cellObject != null)
                    Destroy(kv.Value.cellObject);
            }

            _dictionaryGridCells.Clear();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    GameObject cellObj = Instantiate(gridCellPrefab, transform);
                    cellObj.name = $"Cell_{x}_{y}";
                    cellObj.transform.localPosition = new Vector3(x * cellSpacing, y * cellSpacing, 0);

                    var gridCell = new GridCell
                    {
                        cellObject = cellObj,
                        currentTile = null,
                        position = gridPosition
                    };

                    _dictionaryGridCells.Add(gridPosition, gridCell);
                }
            }

            CenterGridPivot();

            Invoke(nameof(NotifyGridCreated), 0.1f);
        }

        private void NotifyGridCreated()
        {
            EventSystem.ActionGridCreated?.Invoke();
        }

        private void CenterGridPivot()
        {
            float centerOffsetX = ((gridSize.x - 1) * cellSpacing) / 2f;
            float centerOffsetY = ((gridSize.y - 1) * cellSpacing) / 2f;

            Vector3 currentPosition = transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x - centerOffsetX, currentPosition.y - centerOffsetY, currentPosition.z);
            transform.position = newPosition;
        }

        /// <summary>
        /// Boş bir grid hücresi döndürür.
        /// </summary>
        public KeyValuePair<Vector2Int, GridCell>? GetEmptyGridCell()
        {
            foreach (KeyValuePair<Vector2Int, GridCell> cell in _dictionaryGridCells)
            {
                if (cell.Value.currentTile == null)
                    return cell;
            }

            return null;
        }

        /// <summary>
        /// Belirtilen pozisyondaki hücreyi döndürür.
        /// </summary>
        public GridCell GetCell(Vector2Int pos)
        {
            return _dictionaryGridCells.TryGetValue(pos, out GridCell cell) ? cell : null;
        }

        /// <summary>
        /// Belirtilen pozisyona tile yerleştirir.
        /// </summary>
        public void SetTileToCell(Vector2Int pos, Tile tile)
        {
            if (!_dictionaryGridCells.TryGetValue(pos, out GridCell cell))
                return;

            cell.currentTile = tile;
            if (tile != null)
            {
                tile.currentGridPosition = pos;
                tile.transform.position = cell.cellObject.transform.position;
            }
        }

        /// <summary>
        /// Belirtilen pozisyondaki hücreyi temizler.
        /// </summary>
        public void ClearCell(Vector2Int pos)
        {
            if (!_dictionaryGridCells.TryGetValue(pos, out GridCell cell))
                return;

            if (cell.currentTile != null)
            {
                Destroy(cell.currentTile.gameObject);
                cell.currentTile = null;
            }
        }

        /// <summary>
        /// İki hücre arasındaki tile'ları değiştirir.
        /// </summary>
        public void SwapTiles(Vector2Int pos1, Vector2Int pos2)
        {
            GridCell cell1 = GetCell(pos1);
            GridCell cell2 = GetCell(pos2);
            if (cell1 == null || cell2 == null)
                return;

            Tile tempTile = cell1.currentTile;
            cell1.currentTile = cell2.currentTile;
            cell2.currentTile = tempTile;

            if (cell1.currentTile != null)
            {
                cell1.currentTile.currentGridPosition = pos1;
                cell1.currentTile.transform.position = cell1.cellObject.transform.position;
            }

            if (cell2.currentTile != null)
            {
                cell2.currentTile.currentGridPosition = pos2;
                cell2.currentTile.transform.position = cell2.cellObject.transform.position;
            }
        }

        /// <summary>
        /// İki grid pozisyonunun komşu (yatay veya dikey) olup olmadığını kontrol eder.
        /// </summary>
        public bool AreAdjacent(Vector2Int a, Vector2Int b)
        {
            return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
                   (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
        }
    }
}
