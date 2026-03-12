using System;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Uygulama genelinde kullanılan basit event yöneticisi.
    /// Statik olduğundan sahne içinde bir instance’a ihtiyaç yoktur.
    /// </summary>
    public static class EventManager
    {
        public static event Action OnScanStarted;
        public static event Action OnScanFailed;
        public static event Action OnMergeComplete;
        public static event Action OnSpawnComplete;
        public static event Action<TileController, TileController> OnTileSwapped;
        public static event Action<Grid.GridCell[]> OnTilesMatched;

        public static void RaiseScanStarted() => OnScanStarted?.Invoke();
        public static void RaiseScanFailed() => OnScanFailed?.Invoke();
        public static void RaiseMergeComplete() => OnMergeComplete?.Invoke();
        public static void RaiseSpawnComplete() => OnSpawnComplete?.Invoke();
        public static void RaiseTileSwapped(TileController a, TileController b) => OnTileSwapped?.Invoke(a, b);
        public static void RaiseTilesMatched(Grid.GridCell[] cells) => OnTilesMatched?.Invoke(cells);
    }
}
