using System;
using System.Threading;
using Core.Infrastructure.WindowsFsm;
using Core.MVP.Presenters;
using Game.Data.Dto;
using Game.Data.Enums;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;
using Game.Shared.Windows;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Presenters
{
    public class MainUiPresenter : IPresenter, ILateTickable
    {
        private readonly LevelService _levelService;
        private readonly MoneyService _moneyService;
        private readonly MainUiView _view;
        private readonly IWindowFsm _windowFsm;
        private readonly Type _window = typeof(MainUi);

        private CancellationTokenSource _cts;
        private Transform _uiStoneTarget;
        private Transform _uiLeftMoneyTarget;
        private Transform _uiRightMoneyTarget;

        public MainUiPresenter(
            MainUiView view, 
            LevelService levelService, 
            MoneyService moneyService,
            IWindowFsm windowFsm)
        {
            _view = view; 
            _levelService = levelService;
            _moneyService = moneyService;
            _windowFsm = windowFsm;

            _moneyService.UpdateMoney += UpdateMoneyCounter;

            _levelService.UpdateStonesCount += UpdateStonesCount;
            _levelService.ShowProgressBar += ShowProgressBarAsync;
            _levelService.UpdateStonesProgress += UpdateStonesProgress;
            _levelService.ShowStonesProgress += ShowStonesProgress;
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
            
            _moneyService.UpdateMoney -= UpdateMoneyCounter;
            
            _levelService.UpdateStonesCount -= UpdateStonesCount;
            _levelService.ShowProgressBar -= ShowProgressBarAsync;
            _levelService.UpdateStonesProgress -= UpdateStonesProgress;
            _levelService.ShowStonesProgress -= ShowStonesProgress;
            
            _cts?.Dispose();
            _cts = null;
        }
        
        public void LateTick()
        {
            if (_uiStoneTarget != null && _view.StoneProgressPoint.gameObject.activeSelf)
            {
                CalculatePosition(_uiStoneTarget.position, _view.StoneProgressPoint);
            }
        }

        private void CalculatePosition(Vector3 targetPosition, RectTransform pointTransform)
        {
            Vector3 position = _view.MainCamera.WorldToScreenPoint(targetPosition);

            float offset = pointTransform.sizeDelta.x / 2;
            position.x = Mathf.Clamp(position.x, offset, Screen.width - offset);
            position.y = Mathf.Clamp(position.y, offset, Screen.height - offset);
            
            pointTransform.position = position;
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

        private void UpdateMoneyCounter(int value)
        {
            _view.MoneyCountText.text = value.ToString();
        }

        private async void ShowProgressBarAsync(MagazineProgressDto dto, bool isAnimation)
        {
            _view.ProgressBarText.text = $"{dto.Current}/{dto.Max}";
            _view.ProgressBarObject.SetActive(dto.IsActive);
            float fill = (float) dto.Current / dto.Max;
            
            if (isAnimation)
            {
                try
                {
                    _cts = new CancellationTokenSource();
                    await _view.FillAnimation.DoFillAnimationAsync(fill, _cts.Token);
                }
                catch (OperationCanceledException e)
                {
                    _view.ProgressBarFillImage.fillAmount = fill;
                }
                finally
                {
                    _cts?.Dispose();
                    _cts = null;
                }
            }
            _view.ProgressBarFillImage.fillAmount = fill;
        }

        private void UpdateStonesProgress(StonesProgressDto dto)
        {
            if (dto == null)
            {
                ShowStonesProgress(false);
                return;
            }
            
            _view.StoneProgressText.text = dto.Count.ToString();
            _uiStoneTarget = dto.Target;
        }

        private void UpdateMoneyPrices(MoneyPriceDto dto)
        {
            if (dto == null)
            {
                _view.LeftMoneyPoint.gameObject.SetActive(false);
                _view.RightMoneyPoint.gameObject.SetActive(false);
            }
            else if(dto.MagazinesPrices.TryGetValue(MagazineType.Left, out int leftPrice))
            {
                _view.LeftMoneyPoint.gameObject.SetActive(true);
                _view.LeftMoneyText.text = leftPrice.ToString();
                _uiLeftMoneyTarget = dto.LeftTransform;
            }
            else if (dto.MagazinesPrices.TryGetValue(MagazineType.Right, out int rightPrice))
            {
                _view.RightMoneyPoint.gameObject.SetActive(true);
                _view.RightMoneyText.text = rightPrice.ToString();
                _uiRightMoneyTarget = dto.RightTransform;
            }
        }

        private void ShowStonesProgress(bool isActive)
        {
            _view.StoneProgressPoint.gameObject.SetActive(isActive);
        }
    }
}
