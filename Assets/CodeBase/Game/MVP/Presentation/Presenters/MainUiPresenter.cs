using System;
using Core.Infrastructure.WindowsFsm;
using Core.MVP.Presenters;
using Game.Data.Settings;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;
using Game.Shared.Windows;

namespace Game.MVP.Presentation.Presenters
{
    public class MainUiPresenter : IPresenter
    {
        private readonly LevelService _levelService;
        private readonly MainUiView _view;
        private readonly IWindowFsm _windowFsm;
        private readonly GameSettings _gameSettings;
        private readonly Type _window = typeof(MainUi);

        public MainUiPresenter(
            MainUiView view, 
            LevelService levelService, 
            IWindowFsm windowFsm,
            GameSettings settings)
        {
            _view = view; 
            _levelService = levelService;
            _windowFsm = windowFsm;
            _gameSettings = settings;
            
            _levelService.UpdateStonesCount += UpdateStonesCount;
        }
        
        public void Enable()
        {
            _windowFsm.Opened += OnHandleOpenWindow;
            _windowFsm.Closed += OnHandleCloseWindow;
        }

        public void Disable()
        {
            _windowFsm.Opened -= OnHandleOpenWindow;
            _windowFsm.Closed -= OnHandleCloseWindow;
            
            _levelService.UpdateStonesCount -= UpdateStonesCount;
        }
        
        private void OnHandleOpenWindow(Type window)
        {
            if(_window != window || _view == null) return;
            
            _view.StonesCounterObject.SetActive(false);
            _view.ProgressBarObject.SetActive(false);
            _view.Show();
        }
        
        private void OnHandleCloseWindow(Type window)
        {
            if(_window != window || _view == null) return;
            
            _view.Hide();
        }

        private void UpdateStonesCount(int count)
        {
            _view.StonesCounterObject.SetActive(count > 0);
            _view.StonesCountText.text = count.ToString();
        }
    }
}
