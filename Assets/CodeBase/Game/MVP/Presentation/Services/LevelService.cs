using System;
using Core.Infrastructure.WindowsFsm;
using Game.Data.Dto;
using Game.Shared.Windows;

namespace Game.MVP.Presentation.Services
{
    public class LevelService
    {
        private readonly IWindowFsm _windowFsm;

        public LevelService(IWindowFsm windowFsm)
        {
            _windowFsm = windowFsm;
        }
        
        public event Action PrepareLevel;
        public event Action<int> UpdateStonesCount;
        public event Action<int> BuildItem;
        public event Action<MagazineProgressDto, bool> ShowProgressBar;

        public void InvokePrepareLevel()
        {
            _windowFsm.OpenWindow(typeof(MainUi), false);
            PrepareLevel?.Invoke();
        }

        public void InvokeUpdateStonesCount(int count)
        {
            UpdateStonesCount?.Invoke(count);
        }

        public void InvokeBuildItem(int id)
        {
            BuildItem?.Invoke(id);
        }

        public void InvokeShowProgressBar(MagazineProgressDto dto, bool isAnimation)
        {
            ShowProgressBar?.Invoke(dto, isAnimation);
        }

        public void InvokeWinGame()
        {
            _windowFsm.CloseWindow(typeof(MainUi));
            _windowFsm.OpenWindow(typeof(Win), false);
        }
    }
}