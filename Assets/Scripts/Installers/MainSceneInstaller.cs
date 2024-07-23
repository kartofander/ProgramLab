using UnityEngine;
using Zenject;

namespace ProgramLab.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IObjectInfoDisplay>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IObjectsListDisplay>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IToolsButtonsAccessor>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPresenter>().FromComponentInHierarchy().AsSingle();
        }
    }
}