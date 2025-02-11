using UnityEngine;

namespace CodeBase.AssetsManagment
{
    public interface IAssetsProvider
    {
        public void Init();
        public GameObject GetPrefab(string assetName);
    }
}