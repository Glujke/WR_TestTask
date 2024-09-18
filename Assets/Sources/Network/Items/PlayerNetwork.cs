using Mirror;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using WR.Configs;
using WR.Game.Player;
using WR.Game.Interfaces;
using WR.Movement;
using WR.States.Machine;

namespace WR.Network.Items
{
    public class PlayerNetwork : NetworkBehaviour
    {
        private const string MESSAGE_LOSE = "провалил захват флага.";
        private const string MESSAGE_WIN = "одержал победу.";
        private const string CHAT_NAME = "Notifier";

        [SerializeField]
        private PlayerNotifier chat;

        [Inject]
        private ICaptureFlag capturer;
        [Inject]
        private IMovement movement;


        [SyncVar]
        public int Id;
        [SyncVar]
        public int Num;
        [SyncVar]
        public Color Color;
        [SyncVar]
        public string Name;

        public GameObject mainCamera;
        public MeshRenderer rend;

        private void Start()
        {
            mainCamera.SetActive(isOwned);
            rend.material.color = Color;
            name = $"Player [connId={Id}]";
        }

        private void FixedUpdate()
        {
            if (isOwned)
            {
                movement?.Move(transform);
            }
        }

        public void ConnectToFlag(FlagNetwork flag)
        {
            if (!isOwned) return;
            capturer.StartCapturingFlag(flag);
            capturer.onPlayerLoseFlag += SendMessageLose;
        }
        public void DisconnectFromFlag(FlagNetwork flag)
        {
            if (!isOwned) return;
            capturer.StopCapturingFlag(flag);
            capturer.onPlayerLoseFlag -= SendMessageLose;
        }

        [Command]
        public void CmdSendMessageFailed(string msg)
        {
            RpcSendMessageFailed(msg);
        }

        [ClientRpc]
        public void RpcSendMessageFailed(string msg)
        {
            CheckChat();
            chat.SendMessageAllPlayers(msg);
        }

        [Command]
        public void CmdWin()
        {
            var msg = $"{Name} {MESSAGE_WIN}";
            RpcSendWin(msg);
        }

        [ClientRpc]
        public void RpcSendWin(string msg)
        {
            CheckChat();
            chat.ShowWinMessage(msg);
        }

        private void SendMessageLose()
        {
            CmdSendMessageFailed($"{Name} {MESSAGE_LOSE}");
        }

        private void CheckChat()
        {
            if (chat == null)
            {
                chat = GameObject.Find(CHAT_NAME).GetComponent<PlayerNotifier>();
            }
        }
    }
}
