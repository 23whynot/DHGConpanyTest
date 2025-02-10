namespace CodeBase.Core.ObjPool
{
    public interface IPoolableObject
    {
        public bool IsActive {get; }
        public void Activate();
        public void Deactivate();
    }
}