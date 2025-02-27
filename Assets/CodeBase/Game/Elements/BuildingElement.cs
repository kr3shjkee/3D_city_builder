using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data.Enums;
using UnityEngine;

namespace Game.Elements
{
    public class BuildingElement : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private List<BuildingPartElement> _buildObjects;
        [SerializeField] private GameObject _buyPlace;

        private List<Vector3> _positions;
        private BuildingStatus _status = BuildingStatus.NotBuilded;

        public int ID => _id;
        public BuildingStatus Status => _status;
        public GameObject BuyPlace => _buyPlace;
        
        public event Action<int, int> UpdateCounts;

        public void Init()
        {
            _positions = new List<Vector3>();
            
            for (int i = 0; i < _buildObjects.Count; i++)
            {
                Vector3 position = _buildObjects[i].gameObject.transform.localPosition;
                _positions.Add(position);
            }

            foreach (BuildingPartElement item in _buildObjects)
            {
                Vector3 position = new Vector3(
                    item.gameObject.transform.localPosition.x, 
                    item.gameObject.transform.localPosition.y - 10f, 
                    item.gameObject.transform.localPosition.z);

                item.gameObject.transform.localPosition = position;
            }
            InvokeUpdateCounts();
        }

        public void BuildPart()
        {
            BuildingPartElement element =
                _buildObjects.FirstOrDefault(item => item.Status == BuildingStatus.NotBuilded);

            if (element != null)
            {
                int index = _buildObjects.IndexOf(element);
                element.BuildAsync(_positions[index]);
                InvokeUpdateCounts();
            }
        }

        public bool IsFullCompleted()
        {
            return _buildObjects.All(item => item.Status == BuildingStatus.Builded);
        }

        private void InvokeUpdateCounts()
        {
            int allCount = _buildObjects.Count;
            List<BuildingPartElement> completedElements = new List<BuildingPartElement>(_buildObjects.Where(item => item.Status == BuildingStatus.Builded));
            int completedCount = completedElements.Count;

            if (allCount == completedCount)
            {
                _buyPlace.SetActive(false);
                _status = BuildingStatus.Builded;
            }
            UpdateCounts?.Invoke(completedCount, allCount);
        }
    }
}