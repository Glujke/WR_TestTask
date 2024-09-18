using Mirror;
using System.Threading.Tasks;
using WR.Game.Interfaces;
using WR.Network.Info;

namespace WR.Game.Player
{
    public class PlayerSpawner : IPlayerSpawning
    {

        public void SpawnPlayer(PlayerCreateCharacterMessage info)
        {
            NetworkClient.Send(info);
        }
        public async Task SpawnPlayerAsync(PlayerCreateCharacterMessage info)
        {
            NetworkClient.Send(info);
            await WaitForLocalPlayer();
        }
        private async Task WaitForLocalPlayer()
        {
            while (NetworkClient.localPlayer == null)
            {
                await Task.Delay(10);
            }
        }
    }
}
