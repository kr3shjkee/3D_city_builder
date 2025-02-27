using Game.Data.Settings;
using Game.Elements;
using Game.MVP.Presentation.Managers;
using Game.MVP.Presentation.Presenters;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Infrastructure
{
    public class MainInstallerMVP : MonoInstaller
    {
        [SerializeField] private GameSettings _settings;

        [SerializeField] private StoneElement _stoneElement;
        [SerializeField] private int _stonePoolCount;
        
        [SerializeField] private MainUiView _mainUiView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private LevelView _levelView;

        public override void InstallBindings()
        {
            BindSettings();
            BindPools();
            BindServices();
            BindViews();
            BindPresenters();
            BindManagers();
        }

        private void BindSettings()
        {
            Container.BindInstance(_settings);
        }

        private void BindPools()
        {
            Container
                .BindMemoryPool<StoneElement, StoneElement.Pool>()
                .WithInitialSize(_stonePoolCount)
                .FromComponentInNewPrefab(_stoneElement)
                .UnderTransformGroup("Stone Elements Pool");
        }

        private void BindServices()
        {
            Container.Bind<LevelService>().AsSingle(); 
        }

        private void BindViews()
        {
            Container.BindInstance(_mainUiView);
            Container.BindInstance(_playerView);
            Container.BindInstance(_levelView);
        }

        private void BindPresenters()
        {
            Container.Bind<MainUiPresenter>().AsTransient();
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsTransient();
            Container.Bind<LevelPresenter>().AsTransient();
        }

        private void BindManagers()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}