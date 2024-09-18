using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using WR.Configs;
using WR.Game.Interfaces;
using WR.Network.Info;
using WR.Network.Items;

namespace WR.Network.Server
{
    public class CustomNetworkManager : NetworkManager
    {
        public string sceneName = "Game";

        [Inject]
        private LobbyConnectionHandler lobbyConnectionHandler;
        [Inject]
        private IFlagPositionGenerating flagGenerator;
        [Inject]
        private GameConfig gameConfig;
        [Inject]
        private PathConfig pathConfig;
        [Inject]
        private FlagConfig flagConfig;

        public event Action<PlayerConnectionInfo> onConnectToLobby;
        public event Action<List<PlayerConnectionInfo>> onUpdateStatesInLobby;
        public event Action onGameLoad, onGameLoaded;

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<PlayerConnectionInfo>(OnClientUpdateState);
            NetworkServer.RegisterHandler<PlayerCreateCharacterMessage>(OnCreatePlayer);
            NetworkServer.RegisterHandler<FlagCreateMessage>(OnCreateFlags);
        }

        private void OnCreateFlags(NetworkConnectionToClient conn, FlagCreateMessage message)
        {
            var flagPrefab = Resources.Load<GameObject>(pathConfig.PathToFlagPrefab);
            for (int i = 0; i < gameConfig.MaxFlags; i++)
            {
                var pos = flagGenerator.GetRandomPosition();
                GameObject fl = Instantiate(flagPrefab, pos, Quaternion.identity);
                var flag = fl.GetComponent<FlagNetwork>();
                flag.PlayerId = message.id;
                flag.Num = message.num;
                flag.Color = message.color;
                flag.Radius = flagConfig.Radius;
                NetworkServer.Spawn(fl, conn);
            }
        }

        private void OnCreatePlayer(NetworkConnectionToClient conn, PlayerCreateCharacterMessage message)
        {
            var playerObj = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            playerObj.transform.localScale = new Vector3(1, 1, 1);
            playerObj.name = $"Player [connId={conn.connectionId}]";
            var player = playerObj.GetComponent<PlayerNetwork>();
            player.Id = conn.connectionId;
            player.Num = message.num;
            player.Name = message.name;
            player.Color = message.color;
            NetworkServer.AddPlayerForConnection(conn, playerObj);
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            lobbyConnectionHandler.AddConnectionToLobby(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            lobbyConnectionHandler.DeleteConnectionFromLobby(conn);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            NetworkClient.RegisterHandler<PlayerConnectionInfo>(ClientInitialized);
            NetworkClient.RegisterHandler<PlayersConnectionsInfo>(UpdateConnections);
            NetworkClient.RegisterHandler<GameLoadingMessage>(GameLoading);
        }

        private void ClientInitialized(PlayerConnectionInfo info)
        {
            onConnectToLobby?.Invoke(info);
        }


        private void UpdateConnections(PlayersConnectionsInfo info)
        {
            lobbyConnectionHandler.UpdateConnections(info);
            onUpdateStatesInLobby?.Invoke(info.infos);
        }
        public override void OnClientSceneChanged()
        {
            NetworkClient.Ready();
            onGameLoaded?.Invoke();
        }

        private void GameLoading(GameLoadingMessage message)
        {
            onGameLoad?.Invoke();
        }

        private void OnClientUpdateState(NetworkConnectionToClient conn, PlayerConnectionInfo info)
        {
            lobbyConnectionHandler.ClientUpdateState(conn, info);
            if (lobbyConnectionHandler.IsEverybodyReady())
            {
                NetworkServer.SendToAll(new GameLoadingMessage());
                ServerChangeScene(sceneName);
            }
        }
    }
}