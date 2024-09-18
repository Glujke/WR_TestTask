using System;

namespace WR.Game.Interfaces
{
    public interface ICountFlag
    {
        event Action OnAllFlagCapture;
        int Count { get; }
        void FlagAdd();
    }
}
