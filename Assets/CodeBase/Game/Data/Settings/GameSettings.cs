using UnityEngine;

namespace Game.Data.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Create Game Settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
    }
}