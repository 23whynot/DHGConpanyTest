using System;

namespace CodeBase.Controllers.LevelEnd
{
    public interface ILevelEndController
    {
        public event Action OnLevelEnded;
    }
}