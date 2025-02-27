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
        [SerializeField] private List<BuildingElement> _buildingElements;

        private BuildingStatus _status = BuildingStatus.NotBuilded;

        public BuildingStatus Status => _status;
        public event Action<MagazineProgressDto, bool> InvokeShowProgress;

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
            _buildingElements[0].gameObject.SetActive(true);
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
            InvokeShowProgress?.Invoke(dto, isAnimation);
        }
        
        private void SubscribeBuildingElements()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.UpdateCounts += UpdateBuildCount;
            }
        }
        
        private void UnsubscribeBuildingElements()
        {
            foreach (BuildingElement buildingElement in _buildingElements)
            {
                buildingElement.UpdateCounts -= UpdateBuildCount;
            }
        }
        
        private void UpdateBuildCount(int allCount, int completedCount)
        {
            if (allCount == completedCount)
            {
                BuildingElement buildingElement = _buildingElements.FirstOrDefault(item => item.Status == BuildingStatus.NotBuilded);
                if (buildingElement != null)
                {
                    buildingElement.gameObject.SetActive(true);
                }
                else
                {
                    _status = BuildingStatus.Builded;
                }
                ShowProgress(true, true);
            }
            else
            {
                //TODO: Update Counts on Canvas
            }
        }
    }
}