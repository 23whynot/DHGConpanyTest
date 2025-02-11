using CodeBase.AssetsManagment;
using CodeBase.Core.ObjPool;
using CodeBase.Factory;
using CodeBase.Services.Input;
using CodeBase.Services.RendererMaterialService;
using CodeBase.Sphere;
using UnityEngine;
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
            Container.Bind<IInputService>().FromMethod(GetInputService).AsSingle();
            Container.Bind<IMaterialService>().To<MaterialService>().AsSingle();
        }

        private IInputService GetInputService(InjectContext context)
        {
            if (Application.isEditor)
                return new EditorInputService();
            else
                return new MobileInputService();
        }
    }
}
