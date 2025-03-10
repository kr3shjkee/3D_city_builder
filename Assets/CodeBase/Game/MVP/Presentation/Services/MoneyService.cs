using System;
using System.Linq;
using Game.Data.Enums;
using Game.Data.Settings;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Services
{
    public class MoneyService : ITickable
    {
        private readonly SaveLoadService _saveLoadService;
        private readonly GameSettings _gameSettings;

        private int _currentMoney;
        private float _time;
        private bool _isInited;
        
        public MoneyService(SaveLoadService saveLoadService, GameSettings gameSettings)
        {
            _saveLoadService = saveLoadService;
            _gameSettings = gameSettings;
            
        }

        public event Action<int> UpdateMoney;

        public void Init()
        {
            _currentMoney = _saveLoadService.Dto.Money;
            _time = _gameSettings.MoneySettings.TickSeconds;
            UpdateMoney?.Invoke(_currentMoney);
            _isInited = true;
        }

        public void BuyBuild(int value)
        {
            _currentMoney -= value;
            _saveLoadService.SaveMoney(value, false);
            UpdateMoney?.Invoke(_currentMoney);
        }

        public bool GetMagazinePrice(MagazineType type, out int price)
        {
            bool isAvailable = false;
            price = 0;
            if (_saveLoadService.Dto.MagazinesInfo.Exists(item => item.Type == type))
            {
                var magazineInfo = _saveLoadService.Dto.MagazinesInfo.First(magazine => magazine.Type == type);
                isAvailable = magazineInfo.IsBought;
                price = _gameSettings.MoneySettings.BuildingPriceMultiplier * _saveLoadService.Dto.AllCompletedBuilds;
            }
            return !isAvailable;
        }

        public void Tick()
        {
            if(!_isInited)
                return;

            _time -= Time.deltaTime;

            if (_time <= 0f)
            {
                int value = _gameSettings.MoneySettings.MoneyTickMultiplier * _saveLoadService.Dto.AllCompletedBuilds;
                _currentMoney += value;
                _saveLoadService.SaveMoney(value, true);
                UpdateMoney?.Invoke(_currentMoney);
                _time = _gameSettings.MoneySettings.TickSeconds; 
            }
        }
    }
}