using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] GridSystem  gridSystem;    

    void OnEnable()
    {
        EventSystem.ActionGridCreated += FillInAllTheEmptyCells;
    }

    void OnDisable()
    {
        EventSystem.ActionGridCreated -= FillInAllTheEmptyCells;
    }

    void FillInAllTheEmptyCells()
    {
        Debug.Log("Filling in empty cells...");
        int delayCounter = 0;
        float delayIncrement = 0.05f;
        while (true)
        {
            var emptyCell = gridSystem.GetEmptyGridCell();

            if(emptyCell == null)
            {
                break;
            }
            
            SpawnTile(emptyCell , delayCounter * delayIncrement);
            delayCounter++;
        }
    }

    private void SpawnTile(KeyValuePair<Vector2Int, GridSystem.GridCell>? emptyCell , float delay = 0f)
    {
        var _spawnedTile = Pool.Instance.GetPooledObject(PoolType.tile);

        Vector3 gridPosition = emptyCell.Value.Value.grid.transform.position;
        var spawnPosition = new Vector3(gridPosition.x, 20, 0);

        _spawnedTile.transform.position = spawnPosition;

        emptyCell.Value.Value.currentTile = _spawnedTile.GetComponent<Tile>();

        const float overshootValue = .5F;

        DOVirtual.DelayedCall(delay, () =>
        {
            _spawnedTile.transform.DOMove(gridPosition , 30.0F)
            .SetSpeedBased()
            .SetEase(Ease.InBack , overshootValue);
        });
    }
}
