using UnityEngine;

namespace CodeBase.AssetsManagment
{
    public class AssetsProvider : IAssetsProvider
    {
        private GameObject _ballPrefab;

        public void DownloadAllPrefabs()
        {
            _ballPrefab = Resources.Load<GameObject>("Prefabs/PlayerBall");
        }

        public GameObject GetPrefab(string assetName)
        {
            if (assetName == "Ball")
                return _ballPrefab;
            
            return null;
        }
    }
}