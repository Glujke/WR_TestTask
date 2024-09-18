using Mirror;
using Unity.VisualScripting;
using WR.Game.Interfaces;
using WR.Network.Info;
using WR.Network.Items;
using WR.Network.Server;
using WR.States.Machine;

namespace WR.States
{
    public class GameLoadingState : ILoadedState<PlayerConnectionInfo>
    {
        private readonly IStateMachine stateMachine;
        private readonly IPlayerSpawning playerSpawner;
        private readonly CustomNetworkManager networkManager;
        private PlayerConnectionInfo connectionInfo;

        public GameLoadingState(IStateMachine stateMachine, CustomNetworkManager networkManager, IPlayerSpawning playerSpawner) 
        {
            this.stateMachine = stateMachine;
            this.networkManager = networkManager;
            this.playerSpawner = playerSpawner;
        }

        public void Enter(PlayerConnectionInfo load)
        {
            networkManager.onGameLoaded += StartGame;
            connectionInfo = load;
        }

        public void Exit()
        {
            networkManager.onGameLoaded -= StartGame;
        }

        public async void StartGame()
        {
            var loaded = new PlayerCreateCharacterMessage()
            {
                id = connectionInfo.id,
                num = connectionInfo.num,
                name = connectionInfo.name,
                color = connectionInfo.color
            };

            var flags = new FlagCreateMessage()
            {
                id = connectionInfo.id,
                num = connectionInfo.num,
                color = connectionInfo.color
            };

            NetworkClient.Send(flags);
            await playerSpawner.SpawnPlayerAsync(loaded);
            var player = NetworkClient.localPlayer.GetComponent<PlayerNetwork>();
            player.AddComponent<GameScope>();
            stateMachine.Enter<GameState, PlayerNetwork>(player);
        }
    }
}
