using System.Collections.Generic;
using System.Linq;
using Game.Data.Enums;
using UnityEngine;

namespace Game.Elements
{
    public class MagazineElement : MonoBehaviour
    {
        [SerializeField] private MagazineType _type;
        [SerializeField] private List<BuildingElement> _buildingElements;

        public MagazineType Type => _type;

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
                    //TODO: Finish magazine
                }
            }
            else
            {
                //TODO: Update Counts on Canvas
            }
        }
    }
}