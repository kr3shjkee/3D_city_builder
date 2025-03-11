using System.Collections.Generic;
using Core.MVP.Views;
using Game.Elements;
using UnityEngine;

namespace Game.MVP.Presentation.Views
{
    public class LevelView : ViewBase
    {
        [field: SerializeField] public List<MagazineElement> MagazineElements { get; private set; }
        [field: SerializeField] public GameObject BlockObject { get; private set; }
    }
}