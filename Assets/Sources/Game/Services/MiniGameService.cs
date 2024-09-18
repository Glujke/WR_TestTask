using System;
using WR.Configs;
using WR.Game.Interfaces;
using WR.Movement;

namespace WR.Game.Services
{
    public class MiniGameService : IMiniGame
    {
        private readonly MiniGameConfig miniGameConfig;
        private readonly IMovement playerMovement;
        private bool isRun;
        public bool IsRun => isRun;

        public event Action onStart;
        public event Action onStop;
        public event Action onGameWin;
        public event Action onGameLose;

        public MiniGameService(MiniGameConfig miniGameConfig, IMovement playerMovement)
        {
            this.miniGameConfig = miniGameConfig;
            this.playerMovement = playerMovement;
        }

        public void GameLose()
        {
            onGameLose?.Invoke();
            StopGame();
        }

        public void GameWon()
        {
            onGameWin?.Invoke();
            StopGame();
        }

        public void StartGame()
        {
            isRun = true;
            playerMovement.LockControl();
            onStart?.Invoke();
        }

        public void StopGame()
        {
            isRun = false;
            playerMovement.UnlockControl();
            onStop?.Invoke();
        }

        public void OnClickButtonGame(float value)
        {
            if(value >= miniGameConfig.MinValueWin && value <= miniGameConfig.MaxValueWin)
            {
                GameWon();
            } else
            {
                GameLose();
            }
        }
    }
}
