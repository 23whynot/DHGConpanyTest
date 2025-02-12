using UnityEngine;

namespace CodeBase.AssetsManagement
{
    public interface IAssetsProvider
    {
        public void Init();
        public GameObject GetPrefab(string assetName);
    }
}