using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    using Grid;

    /// <summary>
    /// Grid'i tarar ve yatay/dikey 3+ eşleşmeleri bulur.
    /// Sadece bulur, silme işlemini yapmaz.
    /// </summary>
    public class ScanSystem : MonoBehaviour
    {
        /// <summary>
        /// Tarama başlatır ve eşleşmeleri bulursa event fırlatır.
        /// </summary>
        public void StartScan()
        {
            List<GridSystem.GridCell> matches = ScanForMatches();
            if (matches.Count > 0)
            {
                EventSystem.ActionMatchesFound?.Invoke(matches);
            }
        }

        /// <summary>
        /// Tüm grid'i tarar ve eşleşen hücreleri döndürür.
        /// </summary>
        public List<GridSystem.GridCell> ScanForMatches()
        {
            var matches = new List<GridSystem.GridCell>();
            var visited = new HashSet<Vector2Int>();

            foreach (KeyValuePair<Vector2Int, GridSystem.GridCell> kv in GridSystem.Instance.Cells)
            {
                GridSystem.GridCell cell = kv.Value;
                if (cell.currentTile == null || visited.Contains(cell.position))
                    continue;

                // Yatay kontrol
                List<GridSystem.GridCell> horizontalMatches = CheckDirection(cell, Vector2Int.right, visited);
                if (horizontalMatches.Count >= 3)
                    matches.AddRange(horizontalMatches);

                // Dikey kontrol
                List<GridSystem.GridCell> verticalMatches = CheckDirection(cell, Vector2Int.up, visited);
                if (verticalMatches.Count >= 3)
                    matches.AddRange(verticalMatches);
            }

            return matches;
        }

        private List<GridSystem.GridCell> CheckDirection(GridSystem.GridCell start, Vector2Int direction, HashSet<Vector2Int> visited)
        {
            var line = new List<GridSystem.GridCell> { start };
            visited.Add(start.position);

            Vector2Int nextPos = start.position + direction;
            while (GridSystem.Instance.GetCell(nextPos) is var next && next != null &&
                   next.currentTile != null &&
                   next.currentTile.tileType == start.currentTile.tileType)
            {
                line.Add(next);
                visited.Add(next.position);
                nextPos += direction;
            }

            return line;
        }
    }
}
