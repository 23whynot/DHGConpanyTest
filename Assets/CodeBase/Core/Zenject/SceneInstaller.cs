using CodeBase.AssetsManagment;
using CodeBase.Core.ObjPool;
using CodeBase.Factory;
using CodeBase.Sphere;
using Zenject;

namespace CodeBase.Core.Zenject
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
            Container.Bind<ObjectPool>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<IColorOfZoneProvider>().FromComponentInHierarchy().AsSingle();
            
        }
    }
}
