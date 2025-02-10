using CodeBase.AssetsManagment;
using Zenject;

namespace CodeBase.Zenject
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
        }
    }
}
