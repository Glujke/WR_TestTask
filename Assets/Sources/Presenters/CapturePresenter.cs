using System;
using UnityEngine;
using VContainer.Unity;
using WR.Configs;
using WR.Game.Interfaces;
using WR.Network.Items;
using WR.UI;
using Random = UnityEngine.Random;

namespace WR.Presenters
{
    public class CapturePresenter : ITickable, IStartable
    {
        private readonly IShowCapture captureService;
        private readonly BlockFlagView blockFlagView;
        private readonly CaptureView captureView;


        public CapturePresenter(IShowCapture captureService, CaptureView captureView, BlockFlagView blockFlagView)
        {
            this.captureService = captureService;
            this.captureView = captureView;
            this.blockFlagView = blockFlagView;
        }

        public void Start()
        {
            captureView.capture.SetActive(false);
            blockFlagView.capture.SetActive(false);

            captureService.onStartCapture += StartCapture;
            captureService.onStopCapture += StopCapture;
            captureService.onFlagLock += LockFlag;
            captureService.onFlagUnlock += UnlockFlag;
        }

        public void Tick()
        {
            if (captureService.IsCapture)
            {
                if (!captureService.IsLock)
                {
                    captureService.Tick();
                    ShowCapturing();
                } else
                {
                    ShowBlock();
                }
            }
        }

        private void ShowBlock()
        {
            blockFlagView.textTimer.text = ((int)captureService.BlockTimer + 1).ToString();
            blockFlagView.timerImage.fillAmount = 1 - (captureService.BlockTimer / captureService.BlockMaxTime);
        }

        private void ShowCapturing()
        {
            captureView.textTimer.text = ((int)captureService.CaptureTimer + 1).ToString();
            captureView.timerImage.fillAmount = 1 - (captureService.CaptureTimer / captureService.CaptureMaxTime);
        }

        private void StartCapture()
        {
            captureView.capture.SetActive(!captureService.IsLock);
            blockFlagView.capture.SetActive(captureService.IsLock);
        }

        private void StopCapture()
        {
            captureView.capture.SetActive(false);
            blockFlagView.capture.SetActive(false);
        }

        private void LockFlag()
        {
            captureView.capture.SetActive(false);
            blockFlagView.capture.SetActive(true);
        }

        private void UnlockFlag()
        {
            captureView.capture.SetActive(true);
            blockFlagView.capture.SetActive(false);
        }
    }
}
