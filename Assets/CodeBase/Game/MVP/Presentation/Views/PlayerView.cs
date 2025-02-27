using Core.MVP.Views;
using UnityEngine;

namespace Game.MVP.Presentation.Views
{
    public class PlayerView : ViewBase
    {
        [field: SerializeField] public DynamicJoystick Joystick { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        
    }
}