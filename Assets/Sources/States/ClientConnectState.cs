using Mirror;
using System;
using WR.Network.Info;
using WR.Network.Server;
using WR.States.Machine;

namespace WR.States
{
    public class ClientConnectState : ILoadedState<ConnectionData>
    {
        private readonly IStateMachine stateMachine;
        private readonly CustomNetworkManager networkManager;

        public ClientConnectState(IStateMachine stateMachine, CustomNetworkManager networkManager)
        {
            this.stateMachine = stateMachine;
            this.networkManager = networkManager;
        }

        public void Enter(ConnectionData info)
        {
            try
            {
                networkManager.networkAddress = info.ip;

                if (Transport.active is PortTransport portTransport)
                {
                    if (ushort.TryParse(info.port, out ushort port))
                        portTransport.Port = port;
                }
                networkManager.StartClient();
                stateMachine.Enter<LobbyState>();
            }
            catch (Exception ex)
            {
                stateMachine.Enter<ConnectionState, string>(ex.Message);
            }
        }

        public void Exit()
        {
        }
    }
}
