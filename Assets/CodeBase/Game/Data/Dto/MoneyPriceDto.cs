using System.Collections.Generic;
using Game.Data.Enums;
using UnityEngine;

namespace Game.Data.Dto
{
    public class MoneyPriceDto
    {
        public Dictionary<MagazineType, int> MagazinesPrices;
        public Transform LeftTransform;
        public Transform RightTransform;
    }
}