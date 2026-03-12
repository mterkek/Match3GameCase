using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Her tile örneğiyle ilişkilendirilir.
    /// </summary>
    public class TileController : MonoBehaviour
    {
        public ETileType TileType { get; private set; }
        private Vector2Int gridPosition;

        public void Initialize(ETileType type, Vector2Int gridPos)
        {
            TileType = type;
            gridPosition = gridPos;
            transform.position = BoardManager.Instance.GetCell(gridPos).worldPosition;
        }

        public void SetGridPosition(Vector2Int newPos)
        {
            gridPosition = newPos;
        }

        public Vector2Int GridPosition => gridPosition;
    }
}
