using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Tile sınıfı, her tile'ın temel özelliklerini ve davranışlarını yönetir.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        /// <summary>
        /// Tile'ın türü (örneğin, Apple, Orange vb.).
        /// </summary>
        public ETileType tileType;

        /// <summary>
        /// Tile'ın mevcut grid pozisyonu.
        /// </summary>
        public Vector2Int currentGridPosition;

        /// <summary>
        /// Tile'ın sprite renderer'ı.
        /// </summary>
        public SpriteRenderer spriteRenderer;

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Tile'ı başlatır ve gerekli ayarları yapar.
        /// </summary>
        /// <param name="type">Tile türü.</param>
        /// <param name="position">Grid pozisyonu.</param>
        /// <param name="sprite">Tile için kullanılacak sprite.</param>
        public void Init(ETileType type, Vector2Int position, Sprite sprite)
        {
            tileType = type;
            currentGridPosition = position;

            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = sprite;
        }

        /// <summary>
        /// Tile'ın sprite'ını türüne göre değiştirir.
        /// </summary>
        public void SetSprite(Sprite sprite)
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = sprite;
        }

        /// <summary>
        /// Grid pozisyonunu günceller.
        /// </summary>
        public void SetGridPosition(Vector2Int gridPosition)
        {
            currentGridPosition = gridPosition;
        }
    }
}
