using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Game.Data.Dto;
using Game.Data.Enums;
using Game.Data.Settings;
using UnityEngine;
using Zenject;

namespace Game.MVP.Presentation.Services
{
    public class SaveLoadService : IInitializable
    {
        private readonly string _fileName;
        private readonly GameSettings _gameSettings;

        private string _filePath;
        private SaveDto _dto;
        private bool _isLoadingFinish = true;

        public SaveLoadService(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _fileName = _gameSettings.JsonsSettings.SaveJsonName;
        }

        public bool IsLoadingFinish => _isLoadingFinish;
        public SaveDto Dto => _dto;

        public void Initialize()
        {
            _filePath = Application.persistentDataPath + "/" + _fileName;
        }

        public async UniTask TryLoadDataAsync(Action<CallbackType> callback)
        {
            _isLoadingFinish = false;

            if (!File.Exists(_filePath))
            {
                CreateNewDto();
                callback?.Invoke(CallbackType.Loading);
                _isLoadingFinish = true;
                return;
            }

            _dto = JsonUtility.FromJson<SaveDto>(await File.ReadAllTextAsync(_filePath));

            if (_dto == null)
            {
                CreateNewDto();
            }

            callback?.Invoke(CallbackType.Loading);
            _isLoadingFinish = true;
        }

        private void CreateNewDto()
        {
            _dto = new SaveDto
            {
                AllCompletedBuilds = 0,
                Money = _gameSettings.MoneySettings.StartMoney,
                MagazineCompletedBuilds = new Dictionary<MagazineType, int>
                {
                    {MagazineType.Left, 0},
                    {MagazineType.Right, 0}
                }
            };

            SaveData();
        }

        private void SaveData()
        {
            File.WriteAllText(_filePath, JsonUtility.ToJson(_dto));
        }
    }
}