using System.Collections;
using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Sphere;
using UnityEngine;
using Zenject;

namespace CodeBase.Zone
{
    public class ColorZone : IDestroyColorZone
    {
        public Vector3 center;

        public Color color { get; private set; }

        private List<IDestroyableNotifier> _ballsInZone = new List<IDestroyableNotifier>();
        private IDestroyColorZone _destroyColorZoneImplementation;
        private readonly Transform _nonRotationalParent;
        private readonly Material _material;
        private ICoroutineRunner _coroutineRunner;

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");


        public ColorZone(Vector3 center, Color color, Transform nonRotationalParent, Material material, ICoroutineRunner coroutineRunner)
        {
            this.center = center;
            this.color = color;

            _coroutineRunner = coroutineRunner;
            _nonRotationalParent = nonRotationalParent;
            _material = material;
        }

        public void RegisterBall(IDestroyableNotifier ball) => _ballsInZone.Add(ball);

        public void DestroyAllBallsInZone()
        {
           _coroutineRunner.StartCoroutine(DestroyAllBallsInZoneRoutine());
        }

        private IEnumerator DestroyAllBallsInZoneRoutine()
        {
            foreach (IDestroyableNotifier ball in _ballsInZone)
            {
                ball.ZoneDestroy();
                yield return new WaitForSeconds(0.01f);
            }
        }
        

        public Transform GetNonRotationalParent() => _nonRotationalParent;

        public Material GetMaterial()
        {
            _material.SetColor(BaseColor, color);
            return _material;
        }
    }
}