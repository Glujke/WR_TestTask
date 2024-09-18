using System;
using WR.Movement;

namespace WR.Game.Interfaces
{
    public interface IMiniGame
    {
        event Action onStart;
        event Action onStop;
        event Action onGameWin;
        event Action onGameLose;
        void StartGame();
        void StopGame();
        void GameWon();
        void GameLose();
        bool IsRun { get; }
        void OnClickButtonGame(float value);
    }
}
