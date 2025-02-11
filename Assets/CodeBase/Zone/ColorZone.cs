using System.Collections;
using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Services.RendererMaterialService;
using UnityEngine;
using Zenject;

namespace CodeBase.Zone
{
    public class ColorZone : IDestroyColorZone
    {
        public Vector3 Center;

        public Color Color { get; private set; }

        private List<IDestroyableNotifier> _ballsInZone = new List<IDestroyableNotifier>();
        private IDestroyColorZone _destroyColorZoneImplementation;
        private readonly Transform _nonRotationalParent;
        private ICoroutineRunner _coroutineRunner;
        private bool _zoneDestroyed;

        private readonly IMaterialService _materialService;


        public ColorZone(Vector3 center, Color color, Transform nonRotationalParent,
            ICoroutineRunner coroutineRunner, IMaterialService materialService)
        {
            Center = center;
            Color = color;
            _materialService = materialService;
            _coroutineRunner = coroutineRunner;
            _nonRotationalParent = nonRotationalParent;
        }

        public void RegisterBall(IDestroyableNotifier ball) => _ballsInZone.Add(ball);

        public void DestroyAllBallsInZone()
        {
            if (!_zoneDestroyed)
            {
                _zoneDestroyed = true;
                
                _coroutineRunner.StartCoroutine(DestroyAllBallsInZoneRoutine());
                _materialService.DeleteMaterial(Color);
            }
            
        }

        private IEnumerator DestroyAllBallsInZoneRoutine()
        {
            foreach (IDestroyableNotifier ball in _ballsInZone)
            {
                ball.ZoneDestroy();
                yield return new WaitForSeconds(0.003f);
            }
        }

        public Transform GetNonRotationalParent() => _nonRotationalParent;
    }
}