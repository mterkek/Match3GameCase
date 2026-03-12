using System.Collections;
using UnityEngine;

namespace Systems
{
    using Grid;
    using Core;

    /// <summary>
    /// Eşleşen tile’lar için basit patlama/efekt yönetimi.
    /// </summary>
    public class MergeEffectManager : MonoBehaviour
    {
        public static MergeEffectManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            EventManager.OnTilesMatched += HandleTilesMatched;
        }

        private void OnDestroy()
        {
            EventManager.OnTilesMatched -= HandleTilesMatched;
        }

        private void HandleTilesMatched(GridCell[] cells)
        {
            StartCoroutine(PlayEffects(cells));
        }

        private IEnumerator PlayEffects(GridCell[] cells)
        {
            foreach (var cell in cells)
            {
                if (cell.tile != null)
                {
                    // placeholder efekt
                    Destroy(cell.tile.gameObject);
                    cell.tile = null;
                    cell.tileType = default;
                }
            }

            yield return new WaitForSeconds(0.3f);

            EventManager.RaiseMergeComplete();
        }
    }
}
