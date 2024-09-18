using UnityEngine;
using WR.States.Machine;
using WR.UI;

namespace WR.States
{
    public class BootState : IState
    {
        private readonly IStateMachine stateMachine;
        private readonly MainMenuView mainMenu;
        private readonly ServerView server;
        private readonly LobbyView lobbyUI;

        public BootState(IStateMachine stateMachine, MainMenuView mainMenu, ServerView serverUI, LobbyView lobbyUI)
        {
            this.stateMachine = stateMachine;
            this.mainMenu = mainMenu;
            this.server = serverUI;
            this.lobbyUI = lobbyUI;

        }

        public void Enter()
        {
            mainMenu.gameObject.SetActive(false);
            server.gameObject.SetActive(false);
            lobbyUI.gameObject.SetActive(false);
            stateMachine.Enter<ConnectionState>();
        }

        public void Exit()
        {
        }
    }
}
