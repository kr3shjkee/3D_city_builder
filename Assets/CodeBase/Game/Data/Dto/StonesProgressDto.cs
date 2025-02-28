using UnityEngine;

namespace Game.Data.Dto
{
    public class StonesProgressDto
    {
        public readonly int Count;
        public readonly Transform Target;

        public StonesProgressDto(int count, Transform target)
        {
            Count = count;
            Target = target;
        }
    }
}