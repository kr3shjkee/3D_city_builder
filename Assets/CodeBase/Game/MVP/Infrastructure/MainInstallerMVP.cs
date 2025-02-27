using Game.MVP.Presentation.Presenters;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Infrastructure
{
    public class MainInstallerMVP : MonoInstaller
    {
        [SerializeField] private MainUiView _mainUiView;
        [SerializeField] private PlayerView _playerView;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainUiView);
            Container.BindInstance(_playerView);

            Container.Bind<MainUiPresenter>().AsTransient();
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsTransient();
        }
    }
}