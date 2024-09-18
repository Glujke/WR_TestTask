using System;
using WR.Network.Server;
using WR.States.Machine;

namespace WR.States
{
    public class HostConnectState : IState
    {
        private readonly IStateMachine stateMachine;
        private readonly CustomNetworkManager networkManager;
        public HostConnectState(IStateMachine stateMachine, CustomNetworkManager networkManager)
        {
            this.stateMachine = stateMachine;
            this.networkManager = networkManager;
        }
        public void Enter()
        {
            try
            {
                networkManager.StartHost();
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
