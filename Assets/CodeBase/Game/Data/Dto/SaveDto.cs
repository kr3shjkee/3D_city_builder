using System.Collections.Generic;
using Game.Data.Enums;

namespace Game.Data.Dto
{
    public class SaveDto
    {
        public int AllCompletedBuilds;
        public int Money;
        public Dictionary<MagazineType, int> MagazineCompletedBuilds;
    }
}