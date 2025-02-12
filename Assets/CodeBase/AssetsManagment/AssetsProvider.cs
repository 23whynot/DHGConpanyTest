using CodeBase.Factory;
using UnityEngine;

namespace CodeBase.AssetsManagment
{
    public class AssetsProvider : IAssetsProvider
    {
        private GameObject _ballPrefab;
        private GameObject _hudPrefab;

        public void Init() => DownloadAllPrefabs();

        public GameObject GetPrefab(string assetName)
        {
            if (assetName == AssetsProviderPath.Ball)
                return _ballPrefab;
            if (assetName == AssetsProviderPath.Hud)
                return _hudPrefab;
            
            return null;
        }

        private void DownloadAllPrefabs()
        {
            _ballPrefab = Resources.Load<GameObject>(AssetPath.PlayerBall);
            _hudPrefab = Resources.Load<GameObject>(AssetPath.HUD);
            
        }
    }
}