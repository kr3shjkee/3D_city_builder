using System;
using System.Collections.Generic;
using System.Threading;
using Core.MVP.Presenters;
using Cysharp.Threading.Tasks;
using Game.Data.Settings;
using Game.Elements;
using Game.MVP.Presentation.Services;
using Game.MVP.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Presenters
{
    public class PlayerPresenter : IPresenter, IFixedTickable
    {
        private readonly PlayerView _view;
        private readonly LevelService _levelService;
        
        private readonly string _groundedString = "Grounded";
        private readonly string _moveSpeedString = "MoveSpeed";
        
        private readonly PlayerSettings _settings;
        private readonly Stack<StoneElement> _stones;
        private readonly StoneElement.Pool _stonesPool;

        private CancellationTokenSource _cts;
        private bool _isDelay;

        public PlayerPresenter(
            PlayerView view, 
            GameSettings settings, 
            LevelService levelService, 
            StoneElement.Pool stonesPool)
        {
            _view = view;
            _levelService = levelService;
            _stonesPool = stonesPool;
            _settings = settings.PlayerSettings;
            _stones = new Stack<StoneElement>();
        }
        
        public void Enable()
        {
            _levelService.PrepareLevel += SetDefault;
            _view.TriggerEnter += OnTriggerEnterHandler;
            _view.TriggerStay += OnTriggerStayHandlerAsync;
            _view.TriggerExit += OnTriggerExitHandler;
        }

        public void Disable()
        {
            _levelService.PrepareLevel -= SetDefault;
            _view.TriggerEnter += OnTriggerEnterHandler;
            _view.TriggerStay -= OnTriggerStayHandlerAsync;
            _view.TriggerExit -= OnTriggerExitHandler;
        }

        public void FixedTick()
        {
            _view.Animator.SetBool(_groundedString, true);
            
            Vector3 direction = Vector3.right * _view.Joystick.Vertical + Vector3.back * _view.Joystick.Horizontal;

            _view.Rigidbody.velocity = direction * _settings.MoveSpeed;
            
            if (_view.Joystick.Horizontal != 0) 
                _view.transform.rotation = Quaternion.LookRotation(_view.Rigidbody.velocity);
            
            if (_view.Joystick.Vertical != 0)
                _view.Animator.SetFloat(_moveSpeedString, 1f);
            else
                _view.Animator.SetFloat(_moveSpeedString, 0);
        }
        
        private void SetDefault()
        {
            while (_stones.Count > 0)
            {
                StoneElement stoneElement = _stones.Pop();
                _stonesPool.Despawn(stoneElement);
            }

            _view.transform.localPosition = _settings.StartPosition;
        }

        private void OnTriggerEnterHandler(Collider collider)
        {
            
        }
        
        private async void OnTriggerStayHandlerAsync(Collider collider)
        {
            if (collider.TryGetComponent(out StoneGettingElement gettingElement) && _stones.Count < _settings.MaxElementsCount && !_isDelay)
            {
                _isDelay = true;
                StoneElement stone = gettingElement.GetStone();
                stone.MoveAsync(_view.ElementsPlace, new Vector3(0,_stones.Count * _settings.ElementsOffset,0));
                _stones.Push(stone);
                _levelService.InvokeUpdateStonesCount(_stones.Count);
                await UniTask.Delay(TimeSpan.FromSeconds(_settings.Delay));
                _isDelay = false;
            }
        }
        
        private void OnTriggerExitHandler(Collider collider)
        {

        }
    }
}