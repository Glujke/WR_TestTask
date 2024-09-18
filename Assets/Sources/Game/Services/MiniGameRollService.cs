using UnityEngine;
using WR.Configs;
using WR.Game.Interfaces;

namespace WR.Game.Services
{
    public class MiniGameRollService
    {
        private readonly IMiniGame miniGame;
        private readonly MiniGameConfig miniGameConfig;
        private float timer, maxTime;
        public MiniGameRollService(MiniGameConfig config, IMiniGame miniGame)
        {
            this.miniGame = miniGame;
            this.miniGameConfig = config;
            maxTime = miniGameConfig.intervalRollTime;
            timer = maxTime;
        }

        public void Restart()
        {
            timer = maxTime;
        }

        public void TimerTick()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                RollMiniGame();
                timer = maxTime;
            }
        }

        private void RollMiniGame()
        {
            var percent = UnityEngine.Random.Range(0, 100);
            if (percent <= miniGameConfig.chanceInPercent)
            {
                miniGame.StartGame();
            }
        }
    }
}
