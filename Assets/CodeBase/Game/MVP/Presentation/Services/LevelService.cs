using System;
using Core.Infrastructure.WindowsFsm;
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
        
        public event Action<bool> ActivateProgressBar;
        public event Action<float> UpdateProgressBar;
        
        public event Action<int> UpdateStonesCount;

        public void InvokePrepareLevel()
        {
            _windowFsm.OpenWindow(typeof(MainUi), true);
            PrepareLevel?.Invoke();
        }

        public void InvokeActivateProgressBar(bool isActive)
        {
            ActivateProgressBar?.Invoke(isActive);
        }

        public void InvokeUpdateProgressBar(float fill)
        {
            UpdateProgressBar?.Invoke(fill);
        }

        public void InvokeUpdateStonesCount(int count)
        {
            UpdateStonesCount?.Invoke(count);
        }
    }
}