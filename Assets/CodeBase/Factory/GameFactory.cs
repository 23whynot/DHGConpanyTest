using System;
using CodeBase.AssetsManagment;
using CodeBase.Balls.Player;
using CodeBase.Core.ObjPool;
using UnityEngine;
using Zenject;

namespace CodeBase.Factory
{
    public class GameFactory : IGameFactory
    {
        private ObjectPool _objectPool;
        private IAssetsProvider _assetsProvider;
        private readonly int _preLoadCount = 5;
        private IGameFactory _gameFactoryImplementation;

        public event Action PlayerBallGameObjectCreated;

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
            PlayerBallGameObjectCreated?.Invoke();
            return obj;
        }

        private GameObject GetFromPool(object playerBall, Vector3 atPosition)
        {
            PlayerBall obj = _objectPool.GetObject<PlayerBall>();
            obj.transform.position = atPosition;
            return obj.gameObject;
        }

        private void InitAssetsProvider() => _assetsProvider.Init();

        private void RegisterInPool() =>
            _objectPool.RegisterPrefab<PlayerBall>(_assetsProvider.GetPrefab(AssetsProviderPath.Ball), _preLoadCount);
    }
}