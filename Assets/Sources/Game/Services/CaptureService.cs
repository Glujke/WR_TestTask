using System;
using UnityEngine;
using VContainer.Unity;
using WR.Configs;
using WR.Game.Interfaces;
using WR.Network.Items;

namespace WR.Game.Services
{
    public class CaptureService : ICaptureFlag, IShowCapture
    {
        private readonly FlagConfig config;
        private readonly MiniGameRollService miniGameRoll;
        private readonly ICountFlag countFlag;
        private readonly IMiniGame miniGame;
        private float timer, maxTime;
        private FlagNetwork capturedFlag;

        public float CaptureTimer => timer;
        public float CaptureMaxTime => maxTime;

        public float BlockTimer => capturedFlag.Timer;
        public float BlockMaxTime => capturedFlag.MaxTimerBlock;

        public bool IsCapture => capturedFlag != null;
        public bool IsLock => capturedFlag.IsLock;

        public event Action onStartCapture, onStopCapture;
        public event Action onFlagLock, onFlagUnlock;
        public event Action onPlayerLoseFlag;

        public CaptureService(FlagConfig config, MiniGameRollService miniGameRoll,
            ICountFlag countFlag, IMiniGame miniGame)
        {
            this.config = config;
            this.miniGameRoll = miniGameRoll;
            this.countFlag = countFlag;
            this.miniGame = miniGame;
            maxTime = config.MaxTimeToCapture;

            timer = maxTime;
            this.config = config;

            miniGameRoll.Restart();
            miniGame.onGameWin += GameWin;
            miniGame.onGameLose += GameLose;
        }

        public void Restart()
        {
            timer = maxTime;
        }

        public void StartCapturingFlag(FlagNetwork flag)
        {
            timer = maxTime;
            miniGameRoll.Restart();
            capturedFlag = flag;
            flag.onUnlockFlag += UnlockFlag;
            onStartCapture?.Invoke();
        }

        public void StopCapturingFlag(FlagNetwork flag)
        {
            miniGameRoll.Restart();
            timer = maxTime;
            flag.onUnlockFlag -= UnlockFlag;
            capturedFlag = null;
            onPlayerLoseFlag = null;
            onStopCapture?.Invoke();
        }

        public void Tick()
        {
            if (miniGame.IsRun) return;
            Capturing();
        }

        private void Capturing()
        {
            miniGameRoll.TimerTick();
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                CapturedFlag(capturedFlag);
                timer = maxTime;
                StopCapturingFlag(capturedFlag);
                miniGameRoll.Restart();
            }
        }

        private void CapturedFlag(FlagNetwork flag)
        {
            countFlag.FlagAdd();
            flag.onUnlockFlag -= UnlockFlag;
            flag.CmdDestroy();
        }

        private void GameLose()
        {
            LockFlag();
            miniGameRoll.Restart();
            timer = maxTime;
            onPlayerLoseFlag?.Invoke();
        }

        private void LockFlag()
        {
            capturedFlag.LockFlag();
            onFlagLock?.Invoke();
        }

        private void UnlockFlag()
        {
            onFlagUnlock?.Invoke();
        }

        private void GameWin()
        {
        }
    }
}
