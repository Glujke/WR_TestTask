using UnityEngine;
using WR.Network.Info;
using WR.States.Machine;
using WR.UI;

namespace WR.States
{
    public class ConnectionState : IState, ILoadedState<string>
    {
        private readonly MainMenuView mainMenu;
        private readonly IStateMachine stateMachine;

        public ConnectionState(MainMenuView mainMenu, IStateMachine stateMachine)
        {
            this.mainMenu = mainMenu;
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            mainMenu.gameObject.SetActive(true);
            mainMenu.host.onClick.AddListener(StartHost);
            mainMenu.server.onClick.AddListener(StartServer);
            mainMenu.client.onClick.AddListener(StartClient);
        }

        public void Enter(string errorMessage)
        {
            Enter();
            mainMenu.errorMessage.color = Color.red;
            mainMenu.errorMessage.text = errorMessage;
        }

        public void Exit()
        {
            mainMenu.host.onClick.RemoveListener(StartHost);
            mainMenu.server.onClick.RemoveListener(StartServer);
            mainMenu.client.onClick.RemoveListener(StartClient);
            mainMenu.gameObject.SetActive(false);
        }

        private void StartHost()
        {
            stateMachine.Enter<HostConnectState>();
        }

        private void StartClient()
        {
            ConnectionData conn = new ConnectionData()
            {
                ip = mainMenu.ip.text,
                port = mainMenu.port.text
            };
            stateMachine.Enter<ClientConnectState, ConnectionData>(conn);
        }

        private void StartServer()
        {
            stateMachine.Enter<ServerConnectState>();
        }
    }
}
