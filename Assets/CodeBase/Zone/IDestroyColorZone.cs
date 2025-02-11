using UnityEngine;

namespace CodeBase.Zone
{
    public interface IDestroyColorZone
    {
        public void DestroyAllBallsInZone();
        public Transform GetNonRotationalParent();
        public Color Color { get; }
    }
}