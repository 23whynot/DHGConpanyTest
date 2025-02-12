using CodeBase.AssetsManagement;
using CodeBase.Balls.Player;
using CodeBase.Controllers.Renderer;
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
            Container.Bind<IMaterialService>().To<MaterialService>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudInputProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallCountController>().AsSingle();
            Container.Bind<ObjectPool>().AsSingle();
            Container.Bind<IColorOfZoneProvider>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IInputService>().FromMethod(GetInputService).AsSingle();
            Container.Bind<SphereGenerator>().FromComponentInHierarchy().AsSingle();
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

