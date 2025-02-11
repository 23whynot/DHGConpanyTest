using UnityEngine;

namespace CodeBase.Factory
{
    public interface IGameFactory
    {
        public void Init();
        GameObject CreatePlayerBall(Transform at);
    }
}