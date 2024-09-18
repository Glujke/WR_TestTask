using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WR.Network;
using WR.Network.Info;
using WR.Network.Server;
using WR.States.Machine;
using WR.UI;

namespace WR.States
{
    public class LobbyState : IState
    {
        private readonly LobbyView lobbyUI;
        private readonly IStateMachine stateMachine;
        private readonly CustomNetworkManager networkManager;
        private PlayerConnectionInfo myInfo;
        private LobbyPlayerView playerUI;

        public LobbyState(LobbyView lobbyUI, CustomNetworkManager networkManager, IStateMachine stateMachine) 
        {
            this.lobbyUI = lobbyUI;
            this.networkManager = networkManager;
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            lobbyUI.gameObject.SetActive(true);
            lobbyUI.disconnect.onClick.AddListener(Disconnect);
            networkManager.onConnectToLobby += Initialize;
            networkManager.onUpdateStatesInLobby += UpdatePlayers;
            networkManager.onGameLoad += RunGame;
        }

        public void Exit()
        {
            lobbyUI.disconnect.onClick.RemoveListener(Disconnect);
            networkManager.onConnectToLobby -= Initialize;
            networkManager.onUpdateStatesInLobby -= UpdatePlayers;
            networkManager.onGameLoad -= RunGame;
            lobbyUI.gameObject.SetActive(false);
        }

        private void Initialize(PlayerConnectionInfo info)
        {
            this.myInfo = info;
            playerUI = lobbyUI.players[info.num];
            BindButtons();
        }

        private void UpdatePlayers(List<PlayerConnectionInfo> playerInfo)
        {
            int i = 0;
            foreach(var player in lobbyUI.players)
            {
                var isExists = playerInfo.Any(pi => pi.num == i);
                var info = isExists ? playerInfo.SingleOrDefault(pi => pi.num == i) : PlayerConnectionInfo.Default;
                UpdatePlayer(player, info);
                i++;
            }
        }

        private void UpdatePlayer(LobbyPlayerView plUI, PlayerConnectionInfo info)
        {
            plUI.color.color = info.color;
            plUI.playerName.text = info.name;
            plUI.readyText.text = info.id < 0 ? "NOT CONNECTED" : !info.isReady ? "Is Connected, NotReady" : "READY";
            var isMe = info.id == myInfo.id;
            plUI.playerName.interactable = isMe;
            plUI.readyButton.gameObject.SetActive(isMe);
            plUI.changeName.gameObject.SetActive(isMe);
        }


        private void BindButtons()
        {
            playerUI.readyButton.onClick.AddListener(OnClickReady);
            playerUI.changeName.onClick.AddListener(OnChangeName);
        }
        private void UnbindButtons()
        {
            playerUI.readyButton.onClick.RemoveListener(OnClickReady);
            playerUI.changeName.onClick.RemoveListener(OnChangeName);
        }

        private void OnChangeName()
        {
            myInfo.name = playerUI.playerName.text;
            NetworkClient.Send(myInfo);
        }

        private void OnClickReady()
        {
            myInfo.isReady = !myInfo.isReady;
            NetworkClient.Send(myInfo);
        }

        private void Disconnect()
        {
            networkManager.StopClient();
            networkManager.StopHost();
            networkManager.StopServer();
            UnbindButtons();
            stateMachine.Enter<ConnectionState>();
        }

        private void RunGame()
        {
            stateMachine.Enter<GameLoadingState, PlayerConnectionInfo>(myInfo);
        }
    }
}
