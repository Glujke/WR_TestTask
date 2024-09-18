using UnityEngine;
using VContainer.Unity;
using WR.Configs;
using WR.Game.Interfaces;
using WR.UI;

namespace WR.Presenters
{
    public class MiniGamePresenter : ITickable
    {
        private readonly IMiniGame miniGame;
        private readonly MiniGameView miniGameView;
        private readonly float speed;
        private readonly float maxTime;

        private float gameTimer;
        private enum Direction { Left, Right };
        private Direction direction = Direction.Right;



        public MiniGamePresenter(IMiniGame miniGame, MiniGameView miniGameView, MiniGameConfig config)
        {
            this.miniGame = miniGame;
            this.miniGameView = miniGameView;
            speed = config.SpeedValue;
            maxTime = config.MaxTimeGame;
            gameTimer = maxTime;

            miniGameView.buttonClick.onClick.AddListener(ClickGame);
            miniGame.onStart += StartGame;
            miniGame.onStop += StopGame;
            miniGame.StopGame();
        }

        public void Tick()
        {
            if (!miniGame.IsRun) return;

            gameTimer -= Time.deltaTime;
            if (direction == Direction.Right)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
            if (gameTimer <= 0)
            {
                LoseGame();
            }
        }

        private void MoveLeft()
        {
            miniGameView.scrollbar.value -= Time.deltaTime * speed;
            if (miniGameView.scrollbar.value <= 0.0f)
            {
                miniGameView.scrollbar.value = 0.0f;
                direction = Direction.Right;
            }
        }

        private void MoveRight()
        {
            miniGameView.scrollbar.value += Time.deltaTime * speed;
            if (miniGameView.scrollbar.value >= 1.0f)
            {
                miniGameView.scrollbar.value = 1.0f;
                direction = Direction.Left;
            }
        }

        private void StartGame()
        {
            gameTimer = maxTime;
            miniGameView.miniGame.SetActive(true);
            miniGameView.buttonClick.onClick.AddListener(ClickGame);
        }
        private void StopGame()
        {
            miniGameView.miniGame.SetActive(false);
        }
        private void ClickGame()
        {
            miniGame.OnClickButtonGame(miniGameView.scrollbar.value);
            miniGameView.buttonClick.onClick.RemoveListener(ClickGame);
        }

        private void LoseGame()
        {
            miniGame.GameLose();
            gameTimer = maxTime;
        }
    }
}
