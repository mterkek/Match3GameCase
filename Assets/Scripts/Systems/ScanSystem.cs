using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    using Grid;
    using Core;

    /// <summary>
    /// Tüm ızgarayı tarar ve eşleşmeleri bulur.
    /// </summary>
    public class ScanSystem : MonoBehaviour
    {
        public static ScanSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartScan()
        {
            EventManager.RaiseScanStarted();

            var matches = FindMatches();
            if (matches.Count == 0)
            {
                EventManager.RaiseScanFailed();
                return;
            }

            EventManager.RaiseTilesMatched(matches.ToArray());
        }

        private List<BoardManager.GridCell> FindMatches()
        {
            var found = new List<BoardManager.GridCell>();
            var all = BoardManager.Instance.AllCells;

            foreach (var cell in all)
            {
                if (cell.tile == null) continue;
                // Yatay tarama
                CheckDirection(cell, Vector2Int.right, found);
                // Dikey tarama
                CheckDirection(cell, Vector2Int.up, found);
            }

            return found;
        }

        private void CheckDirection(BoardManager.GridCell start, Vector2Int dir, List<BoardManager.GridCell> list)
        {
            var line = new List<BoardManager.GridCell> { start };
            var nextPos = start.gridPos + dir;

            while (BoardManager.Instance.GetCell(nextPos) is var next && next.tile != null &&
                   next.tileType == start.tileType)
            {
                line.Add(next);
                nextPos += dir;
            }

            if (line.Count >= 3)
                list.AddRange(line);
        }
    }
}
