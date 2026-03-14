using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Systems
{
    using Grid;

    /// <summary>
    /// Mevcut tile'ları boşluklara doğru aşağı indirir.
    /// Animasyon bitince refill çağırır.
    /// </summary>
    public class FallSystem : MonoBehaviour
    {
        [SerializeField] private float fallDuration = 0.3f;

        private void OnEnable()
        {
            EventSystem.ActionFillInEmptyCells += FallTiles;
        }

        private void OnDisable()
        {
            EventSystem.ActionFillInEmptyCells -= FallTiles;
        }

        private void FallTiles()
        {
            var gridSize = GridSystem.Instance.GridSize;

            for (int x = 0; x < gridSize.x; x++)
            {
                FallColumn(x, gridSize.y);
            }

            // Tüm fall animasyonları bitince refill çağır
            DOVirtual.DelayedCall(fallDuration, () => EventSystem.ActionRefill?.Invoke());
        }

        private void FallColumn(int x, int height)
        {
            var tilesInColumn = new List<Tile>();
            var emptyCount = 0;

            // Aşağıdan yukarıya tara
            for (int y = 0; y < height; y++)
            {
                GridSystem.GridCell cell = GridSystem.Instance.GetCell(new Vector2Int(x, y));
                if (cell != null && cell.currentTile != null)
                {
                    tilesInColumn.Add(cell.currentTile);
                }
                else
                {
                    emptyCount++;
                }
            }

            // Tile'ları aşağı indir
            for (int i = 0; i < tilesInColumn.Count; i++)
            {
                Tile tile = tilesInColumn[i];
                var newY = emptyCount + i;
                var newPos = new Vector2Int(x, newY);
                GridSystem.GridCell targetCell = GridSystem.Instance.GetCell(newPos);

                if (targetCell != null)
                {
                    GridSystem.Instance.SetTileToCell(newPos, tile);
                    tile.transform.DOMove(targetCell.cellObject.transform.position, fallDuration).SetEase(Ease.InQuad);
                }
            }
        }
    }
}
