using System;
using Game.Data.Enums;

namespace Game.Data
{
    [Serializable]
    public class MagazineInfo
    {
        public readonly MagazineType Type;
        
        private bool _isBought;
        private int _completedBuilds;
        private int _currentBuildParts;

        public MagazineInfo(MagazineType type)
        {
            Type = type;
        }

        public bool IsBought => _isBought;
        public int CompletedBuilds => _completedBuilds;

        public int CurrentBuildParts => _currentBuildParts;

        public void GetBoughtValue(bool isBought)
        {
            _isBought = isBought;
        }

        public void UpdateCompletedBuildValue(int count)
        {
            _completedBuilds = count;
        }

        public void UpdateCurrentBuildParts(int count)
        {
            _currentBuildParts = count;
        }
    }
}