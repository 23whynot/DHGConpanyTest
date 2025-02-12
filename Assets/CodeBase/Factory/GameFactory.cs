using System;
using CodeBase.AssetsManagement;
using CodeBase.Balls.Player;
using CodeBase.Core.ObjPool;
using UnityEngine;
using Zenject;

namespace CodeBase.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly int _preLoadCount = 5;

        private ObjectPool _objectPool;
        private IAssetsProvider _assetsProvider;

        [Inject]
        public void Construct(ObjectPool objectPool, IAssetsProvider assetsProvider)
        {
            _objectPool = objectPool;
            _assetsProvider = assetsProvider;
        }

        public void Init()
        {
            InitAssetsProvider();
            RegisterInPool();
        }

        public GameObject CreatePlayerBall(Transform at)
        {
            GameObject obj = GetFromPool(AssetPath.PlayerBall, at.position);
            return obj;
        }

        private GameObject GetFromPool(object playerBall, Vector3 atPosition)
        {
            PlayerBall obj = _objectPool.GetObject<PlayerBall>();
            obj.transform.position = atPosition;
            obj.Activate();
            return obj.gameObject;
        }

        private void InitAssetsProvider() => _assetsProvider.Init();

        private void RegisterInPool() =>
            _objectPool.RegisterPrefab<PlayerBall>(_assetsProvider.GetPrefab(AssetsProviderPath.Ball), _preLoadCount);
    }
}