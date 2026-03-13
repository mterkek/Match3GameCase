using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public class GridCell
    {
        public GameObject grid;
        public Tile currentTile;

    }
    private Dictionary<Vector2Int, GridCell> _dictionaryGridCells = new Dictionary<Vector2Int, GridCell>();

    [Header("Grid Settings")]
    [SerializeField] private GameObject gridCellPrefab;

    void OnEnable()
    {
        CreateGrid(new Vector2Int(6, 6));
    }

    private void CreateGrid(Vector2Int gridSize)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                GameObject cell = Instantiate(gridCellPrefab, transform);
                cell.name = $"Cell_{x}_{y}";
                cell.transform.localPosition = new Vector3(x, y, 0);
                GridCell gridCell = new GridCell
                {
                    grid = cell,
                    currentTile = null
                };
                _dictionaryGridCells.Add(gridPosition, gridCell);
            }
        }

        GridPositionUpdateFromCenterPivot();

        DOVirtual.DelayedCall(.1F, () =>
        {
            EventSystem.ActionGridCreated?.Invoke();
        });

    }

    void GridPositionUpdateFromCenterPivot()
    {
        const float gridSpacing = 1f; // Assuming each cell is 1 unit apart

        float centerOffsetX = (5 * gridSpacing) / 2f; // Center pivot offset 
        float centerOffsetY = (5 * gridSpacing) / 2f; // Center pivot offset

        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x - centerOffsetX, currentPosition.y - centerOffsetY, currentPosition.z);
        transform.position = newPosition;

    }

    public KeyValuePair<Vector2Int, GridCell>? GetEmptyGridCell()
    {
        KeyValuePair<Vector2Int, GridCell>? emptyCell = null;

        foreach (var cell in _dictionaryGridCells)
        {
            if (cell.Value.currentTile == null)
            {
                emptyCell = cell;
                break;
            }
        }

        return emptyCell;
    }
}
