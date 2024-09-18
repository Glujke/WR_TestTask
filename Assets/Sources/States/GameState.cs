using System.Reflection;
using WR.Game.Interfaces;
using WR.Network.Items;
using WR.States.Machine;

namespace WR.States
{
    public class GameState : ILoadedState<PlayerNetwork>
    {
        private readonly ICountFlag countFlag;
        private readonly IStateMachine stateMachine;
        private PlayerNetwork playerNetwork;
        public GameState(IStateMachine stateMachine, ICountFlag countFlag)
        {
            this.countFlag = countFlag;
            this.stateMachine = stateMachine;
        }

        public void Enter(PlayerNetwork player)
        {
            playerNetwork = player;
            countFlag.OnAllFlagCapture += EndGame;
        }

        public void Exit()
        {
            countFlag.OnAllFlagCapture -= EndGame;
        }
        private void EndGame()
        {
            playerNetwork.CmdWin();
        }
    }
}
