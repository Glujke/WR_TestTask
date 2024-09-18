using WR.Network.Server;
using WR.States.Machine;
using WR.UI;

namespace WR.States
{
    public class ServerListenState : IState
    {
        private readonly CustomNetworkManager networkManager;
        private readonly ServerView serverUI;
        private readonly IStateMachine stateMachine;

        public ServerListenState(ServerView serverUI, CustomNetworkManager networkManager, IStateMachine stateMachine) 
        {
            this.networkManager = networkManager;
            this.serverUI = serverUI;
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            serverUI.gameObject.SetActive(true);
            serverUI.Disconnect.onClick.AddListener(StopServer);
        }

        public void Exit()
        {
            serverUI.Disconnect.onClick.RemoveListener(StopServer);
            serverUI.gameObject.SetActive(false);
        }

        private void StopServer()
        {
            networkManager.StopServer();
            stateMachine.Enter<ConnectionState>();

        }
    }
}
