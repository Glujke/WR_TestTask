using System;
using WR.Network.Items;

namespace WR.Game.Interfaces
{
    public interface ICaptureFlag
    {
        void StartCapturingFlag(FlagNetwork flag);
        void StopCapturingFlag(FlagNetwork flag);

        event Action onPlayerLoseFlag;
    }
}
