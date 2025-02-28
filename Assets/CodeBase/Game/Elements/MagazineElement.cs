using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data.Dto;
using Game.Data.Enums;
using UnityEngine;

namespace Game.Elements
{
    public class MagazineElement : MonoBehaviour
    {
        [SerializeField] private MagazineType _type;
        [SerializeField] private List<BuildingElement> _buildingElements;

        private BuildingStatus _status = BuildingStatus.NotBuilded;
        private BuildingElement _currentBuildingElement;

        public MagazineType Type => _type;
        public BuildingStatus Status => _status;
        public event Action<MagazineProgressDto, bool> InvokeShowMagazineProgress;
        public event Action<StonesProgressDto> InvokeStonesProgress;

        private void Awake()
        {
            SubscribeBuildingElements();
        }

        private void OnDestroy()
        {
            UnsubscribeBuildingElements();
        }

        public void Init()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.Init();
                buildingElement.gameObject.SetActive(false);
            }
            _currentBuildingElement = _buildingElements[0];
            _currentBuildingElement.gameObject.SetActive(true);
        }

        public void BuildPart(int id)
        {
            BuildingElement buildingElement = _buildingElements.FirstOrDefault(item => item.ID == id);
            if (buildingElement != null)
            {
                buildingElement.BuildPart();
            }
        }

        public void ShowProgress(bool isActive, bool isAnimation)
        {
            int current = _buildingElements.Count(item => item.IsFullCompleted());
            int max = _buildingElements.Count;
            MagazineProgressDto dto = new MagazineProgressDto(current, max, isActive);
            InvokeShowMagazineProgress?.Invoke(dto, isAnimation);
        }
        
        public void UpdateStonesProgress()
        {
            if (_currentBuildingElement != null)
            {
                int needStones = _currentBuildingElement.NeedStones();
                StonesProgressDto dto = new StonesProgressDto(needStones, _currentBuildingElement.BuyPlace);
                InvokeStonesProgress?.Invoke(dto);
            }
        }
        
        private void SubscribeBuildingElements()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.UpdateStoneProgress += UpdateStoneBuildProgress;
            }
        }
        
        private void UnsubscribeBuildingElements()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.UpdateStoneProgress -= UpdateStoneBuildProgress;
            }
        }
        
        private void UpdateStoneBuildProgress(int allCount, int completedCount)
        {
            if (allCount == completedCount)
            {
                _currentBuildingElement = _buildingElements.FirstOrDefault(item => item.Status == BuildingStatus.NotBuilded);
                if (_currentBuildingElement != null)
                {
                    _currentBuildingElement.gameObject.SetActive(true);
                }
                else
                {
                    _status = BuildingStatus.Builded;
                    InvokeStonesProgress?.Invoke(null);
                }
                ShowProgress(true, true);
            }

            UpdateStonesProgress();
        }
    }
}