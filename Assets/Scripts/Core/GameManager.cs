using UnityEngine;

namespace Core
{
    /// <summary>
    /// Oyun başlangıcını ve temel event aboneliklerini yönetir.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            // Gerekirse başka servisleri başlatma yeri.
        }

        private void Start()
        {
            // Event abonelikleri. 
            EventManager.OnMergeComplete += HandleMergeComplete;
            EventManager.OnSpawnComplete += HandleSpawnComplete;
        }

        private void OnDestroy()
        {
            EventManager.OnMergeComplete -= HandleMergeComplete;
            EventManager.OnSpawnComplete -= HandleSpawnComplete;
        }

        private void HandleMergeComplete()
        {
            // Boşluklar oluştu, yeni tile’ları spawn et.
            Systems.TileSpawner.Instance.SpawnForEmptyCells();
        }

        private void HandleSpawnComplete()
        {
            // Yeni tile’lar yerleşti, yeniden tarama başlat.
            Systems.ScanSystem.Instance.StartScan();
        }
    }
}
