using UnityEngine;

namespace Input
{
    using Grid;
    using Systems;

    /// <summary>
    /// Basit click-to-swap input yöneticisi.
    /// İlk tıklamada bir tile seçer, ikinci tıklamada seçili tile ile komşuysa swap yapar.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private Tile _firstSelected;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider == null)
                return;

            if (!hit.collider.TryGetComponent<Tile>(out Tile tile))
                return;

            OnTileClicked(tile);
        }

        private void OnTileClicked(Tile tile)
        {
            if (_firstSelected == null)
            {
                _firstSelected = tile;
                HighlightTile(_firstSelected, true);
                return;
            }

            if (_firstSelected == tile)
            {
                HighlightTile(_firstSelected, false);
                _firstSelected = null;
                return;
            }

            if (GridSystem.Instance.AreAdjacent(_firstSelected.currentGridPosition, tile.currentGridPosition))
            {
                GridSystem.Instance.SwapTiles(_firstSelected.currentGridPosition, tile.currentGridPosition);
                // Swap sonrası scan başlat
                FindObjectOfType<ScanSystem>().StartScan();
            }

            HighlightTile(_firstSelected, false);
            _firstSelected = null;
        }

        private void HighlightTile(Tile tile, bool highlight)
        {
            if (tile == null) return;
            SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
            if (renderer == null) return;

            renderer.color = highlight ? Color.yellow : Color.white;
        }
    }
}
