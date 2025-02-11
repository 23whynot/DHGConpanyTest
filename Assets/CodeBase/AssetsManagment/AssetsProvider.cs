using CodeBase.Factory;
using UnityEngine;

namespace CodeBase.AssetsManagment
{
    public class AssetsProvider : IAssetsProvider
    {
        private GameObject _ballPrefab;

        private const string Ball = "Ball";

        public void Init() => DownloadAllPrefabs();

        public GameObject GetPrefab(string assetName)
        {
            if (assetName == Ball)
                return _ballPrefab;
            
            return null;
        }

        private void DownloadAllPrefabs()
        {
            _ballPrefab = Resources.Load<GameObject>(AssetPath.PlayerBall);
        }
    }
}