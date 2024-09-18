using System;
using WR.Configs;
using WR.Game.Interfaces;

namespace WR.Game.Services
{
    public class FlagCounterService : ICountFlag
    {
        private readonly GameConfig gameConfig;
        private int count;

        public event Action OnAllFlagCapture;

        public FlagCounterService(GameConfig gameConfig) 
        {
            this.gameConfig = gameConfig;
            count = gameConfig.MaxFlags;
        }

        public int Count => count;

        public void FlagAdd()
        {
            count--;
            if (count <= 0) OnAllFlagCapture?.Invoke();
        }
    }
}
