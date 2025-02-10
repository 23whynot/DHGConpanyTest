using UnityEngine;

namespace CodeBase.AssetsManagment
{
    public interface IAssetsProvider
    {
        public GameObject GetPrefab(string assetName);
    }
}