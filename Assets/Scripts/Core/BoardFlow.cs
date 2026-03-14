using UnityEngine;

namespace Core
{
    /// <summary>
    /// Board akışını yönetir: swap → scan → clear → fall → refill → rescan
    /// </summary>
    public class BoardFlow : MonoBehaviour
    {
        private void OnEnable()
        {
            // Swap sonrası scan başlat (InputHandler'dan çağrılır)
            // Scan sonrası matches found (ScanSystem'den)
            // Matches found sonrası clear (MatchResolver'dan)
            // Clear sonrası fall (MatchResolver'dan ActionFillInEmptyCells)
            // Fall sonrası refill (FallSystem'den ActionRefill)
            // Refill sonrası spawn completed (TileSpawner'dan)
            // Spawn completed sonrası rescan (GameManager'dan)
        }

        // Bu sınıf event'leri dinlemeden, sadece akışı temsil ediyor.
        // Gerçek akış event'lerle yönetiliyor.
    }
}