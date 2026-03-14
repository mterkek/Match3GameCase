using UnityEngine;

namespace Core
{
    using Systems;

    /// <summary>
    /// Oyun genelinde event zincirini yönetir.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventSystem.ActionSpawnCompleted += OnSpawnCompleted;
        }

        private void OnDisable()
        {
            EventSystem.ActionSpawnCompleted -= OnSpawnCompleted;
        }

        private void OnSpawnCompleted()
        {
            // Spawn tamamlandıktan sonra tekrar scan başlat (zincir devam etsin)
            FindObjectOfType<ScanSystem>().StartScan();
        }
    }
}