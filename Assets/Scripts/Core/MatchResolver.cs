using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    using Grid;

    /// <summary>
    /// Scan'den gelen eşleşmeleri işler, hücreleri boşaltır, tile'ları pool'a geri gönderir ve gravity'yi çağırır.
    /// </summary>
    public class MatchResolver : MonoBehaviour
    {
        private void OnEnable()
        {
            EventSystem.ActionMatchesFound += ResolveMatches;
        }

        private void OnDisable()
        {
            EventSystem.ActionMatchesFound -= ResolveMatches;
        }

        /// <summary>
        /// Eşleşen hücreleri işler.
        /// </summary>
        public void ResolveMatches(List<GridSystem.GridCell> matches)
        {
            foreach (GridSystem.GridCell cell in matches)
            {
                if (cell.currentTile != null)
                {
                    // Tile'ı pool'a geri gönder
                    Pool.Instance.ReturnToPool(cell.currentTile.gameObject);
                    // Hücreyi boşalt
                    cell.currentTile = null;
                }
            }

            // Gravity'yi çağır (boş hücreleri doldur)
            EventSystem.ActionFillInEmptyCells?.Invoke();
        }
    }
}
