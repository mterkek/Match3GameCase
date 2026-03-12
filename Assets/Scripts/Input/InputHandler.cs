using UnityEngine;

namespace Input
{
    using Grid;
    using Core;

    /// <summary>
    /// Mouse ile tile seçme, sürükleme ve swap işlemlerini sağlar.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        private Camera cam;
        private TileController selected;

        private bool inputEnabled = true;

        private void Awake()
        {
            cam = Camera.main;
            EventManager.OnScanStarted += DisableInput;
            EventManager.OnScanFailed += EnableInput;
            EventManager.OnMergeComplete += EnableInput;
        }

        private void OnDestroy()
        {
            EventManager.OnScanStarted -= DisableInput;
            EventManager.OnScanFailed -= EnableInput;
            EventManager.OnMergeComplete -= EnableInput;
        }

        private void Update()
        {
            if (!inputEnabled) return;

            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    selected = hit.collider.GetComponent<TileController>();
                }
            }

            if (Input.GetMouseButtonUp(0) && selected != null)
            {
                var hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    var other = hit.collider.GetComponent<TileController>();
                    TrySwap(selected, other);
                }

                selected = null;
            }
        }

        private void TrySwap(TileController a, TileController b)
        {
            if (a == null || b == null) return;

            var amaybe = BoardManager.Instance.GetCell(a.GridPosition);
            var bmaybe = BoardManager.Instance.GetCell(b.GridPosition);

            if (BoardManager.Instance.AreAdjacent(amaybe.gridPos, bmaybe.gridPos))
            {
                DisableInput();
                BoardManager.Instance.SwapTiles(amaybe, bmaybe);
                EventManager.RaiseScanStarted();
                Systems.ScanSystem.Instance.StartScan();
            }
        }

        private void DisableInput() => inputEnabled = false;
        private void EnableInput() => inputEnabled = true;
    }
}
