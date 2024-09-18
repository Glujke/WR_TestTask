using Mirror;
using System;
using UnityEngine;

namespace WR.Network.Items
{
    public class FlagNetwork : NetworkBehaviour
    {
        private PlayerNetwork player;
        private bool IsConnect = false;
        private bool isLock = false;
        private bool isInit = false;
        private float timer;
        private float maxTimerBlock;

        [SyncVar]
        public int PlayerId;
        [SyncVar] 
        public int Num;
        [SyncVar]
        public Color Color;
        [SyncVar]
        public float Radius;

        public bool IsLock => isLock;
        public float Timer => timer;
        public float MaxTimerBlock => maxTimerBlock;

        public MeshRenderer rend;
        public MeshRenderer rendRadius;

        public event Action onUnlockFlag;


        public void Init(PlayerNetwork player, float maxTimerBlock)
        {
            this.player = player;
            isInit = true;
            this.maxTimerBlock = maxTimerBlock;
            timer = maxTimerBlock;
        }

        private void Start()
        {
            rend.material.color = Color;
            rendRadius.material.color = Color;
            transform.Find("Radius").localScale = new Vector3(Radius * 2, 0.1f, Radius * 2);
        }

        private void Update()
        {
            if (!isInit) return;
            if (IsLock)
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    isLock = false;
                    timer = maxTimerBlock;
                    onUnlockFlag?.Invoke();
                }
            }
            if (IsConnect && Vector3.Distance(player.transform.position, transform.position) >= Radius) DisconnectFromFlag();
            if(!IsConnect && Vector3.Distance(player.transform.position, transform.position) < Radius) ConnectToFlag();
        }

        public void LockFlag()
        {
            isLock = true;
        }

        private void ConnectToFlag()
        {
            IsConnect = true;
            player.ConnectToFlag(this);
        }

        private void DisconnectFromFlag()
        {
            IsConnect = false;
            player.DisconnectFromFlag(this);
        }

        [Command]
        public void CmdDestroy()
        {
            NetworkServer.Destroy(this.gameObject);
        }

        [ClientRpc]
        public void RpcDestroy(GameObject go)
        {
            Destroy(go);
        }
    }
}
