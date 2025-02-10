using CodeBase.AssetsManagment;
using CodeBase.Core.ObjPool;
using CodeBase.Sphere;
using Zenject;

namespace CodeBase.Zenject
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
            Container.Bind<ObjectPool>().AsSingle();
            Container.Bind<IColorOfZoneProvider>().FromComponentInHierarchy().AsSingle();
            
        }
    }
}
