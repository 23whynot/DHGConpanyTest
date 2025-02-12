using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Services.RendererMaterialService;
using UnityEngine;


namespace CodeBase.Zone
{
    public class ColorZone : IColorZone
    {
        public Vector3 Center;

        public event Action<ColorZone> OnZoneDestroyed; 

        public Color Color { get; private set; }

        private List<IDestroyableNotifier> _ballsInZone = new List<IDestroyableNotifier>();
        private IColorZone _colorZoneImplementation;
        private readonly Transform _nonRotationalParent;
        private ICoroutineRunner _coroutineRunner;
        private bool _zoneDestroyed;
        private const float CascadeTime = 0.0005f;

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
                yield return new WaitForSeconds(CascadeTime);
            }
            OnZoneDestroyed?.Invoke(this);
        }

        public Transform GetNonRotationalParent() => _nonRotationalParent;
    }
}