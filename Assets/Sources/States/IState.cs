namespace WR.States
{

    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface ILoadedState<TLoad> : IExitableState
    {
        void Enter(TLoad load);
    }
    public interface IExitableState
    {
        void Exit();
    }
}