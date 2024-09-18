using System;

namespace WR.Game.Interfaces
{
    public interface IShowCapture
    {
        float CaptureTimer { get; }
        float CaptureMaxTime { get; }

        float BlockTimer { get; }
        float BlockMaxTime { get; }

        bool IsCapture { get; }
        bool IsLock { get; }

        event Action onStartCapture, onStopCapture;
        event Action onFlagLock, onFlagUnlock;
        void Tick();
    }
}