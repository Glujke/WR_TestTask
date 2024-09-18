
using System.Threading.Tasks;
using WR.Network.Info;

namespace WR.Game.Interfaces
{
    public interface IPlayerSpawning
    {
        void SpawnPlayer(PlayerCreateCharacterMessage info);
        Task SpawnPlayerAsync(PlayerCreateCharacterMessage info);
    }
}
