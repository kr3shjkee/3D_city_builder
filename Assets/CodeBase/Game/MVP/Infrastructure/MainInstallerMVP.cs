using Game.MVP.Presentation.Presenters;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Infrastructure
{
    public class MainInstallerMVP : MonoInstaller
    {
        [SerializeField] private MainUiView _mainUiView;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainUiView);

            Container.Bind<MainUiPresenter>().AsTransient();
        }
    }
}