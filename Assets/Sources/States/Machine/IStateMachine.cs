namespace WR.States.Machine

{
    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TLoad>(TLoad payLoad) where TState : class, ILoadedState<TLoad>;
    }
}