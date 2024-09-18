
using System;
using WR.Network.Server;
using WR.States.Machine;

namespace WR.States
{
    public class ServerConnectState : IState
    {
        private readonly IStateMachine stateMachine;
        private readonly CustomNetworkManager networkManager;
        public ServerConnectState(IStateMachine stateMachine, CustomNetworkManager networkManager)
        {
            this.stateMachine = stateMachine;
            this.networkManager = networkManager;
        }
        public void Enter()
        {
            try
            {
                networkManager.StartServer();
                stateMachine.Enter<ServerListenState>();
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
