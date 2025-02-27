using Game.MVP.Presentation.Services;
using Zenject;

namespace Game.MVP.Presentation.Managers
{
    public class GameManager : IInitializable
    {
        private readonly LevelService _levelService;

        public GameManager(LevelService levelService)
        {
            _levelService = levelService;
        }
        
        public void Initialize()
        {
            // TODO: Load Logic
            _levelService.InvokePrepareLevel();
        }
    }
}