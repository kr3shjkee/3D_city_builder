using Core.MVP.Views;
using TMPro;
using UnityEngine;

namespace Game.MVP.Presentation.Views
{
    public class MainUiView : CanvasGroupView
    {
        [field: SerializeField] public TMP_Text BoxesText { get; private set; }
        [field: SerializeField] public GameObject BoxesCounterObject { get; private set; }
    }
}
