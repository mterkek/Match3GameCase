using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// 2D grid verisini saklar ve hücre/swap işlemlerini sağlar.
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager Instance { get; private set; }

        [SerializeField] private int width = 8;
        [SerializeField] private int height = 8;
        [SerializeField] private float cellSize = 1f;

        private Dictionary<Vector2Int, GridCell> cells;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            cells = new Dictionary<Vector2Int, GridCell>(width * height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector2Int(x, y);
                cells[pos] = new GridCell
                {
                    gridPos = pos,
                    worldPosition = new Vector3(x * cellSize, y * cellSize, 0),
                    tile = null,
                    tileType = default
                };
            }
        }

        public GridCell GetCell(Vector2Int pos) => cells.TryGetValue(pos, out var cell) ? cell : null;

        public IEnumerable<GridCell> AllCells => cells.Values;

        public bool AreAdjacent(Vector2Int a, Vector2Int b) =>
            (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
            (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);

        public void SwapTiles(GridCell a, GridCell b)
        {
            var ta = a.tile;
            var tb = b.tile;

            if (ta == null || tb == null) return;

            a.tile = tb;
            a.tileType = tb.TileType;
            b.tile = ta;
            b.tileType = ta.TileType;

            // Pozisyonları güncelle
            ta.SetGridPosition(b.gridPos);
            tb.SetGridPosition(a.gridPos);

            EventManager.RaiseTileSwapped(ta, tb);
        }

        public struct GridCell
        {
            public Vector2Int gridPos;
            public Vector3 worldPosition;
            public TileController tile;
            public ETileType tileType;
        }
    }
}
