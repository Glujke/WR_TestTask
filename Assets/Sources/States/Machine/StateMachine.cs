using System;
using System.Collections.Generic;
using System.Linq;

namespace WR.States.Machine

{
    public class StateMachine : IStateMachine
    {
        private Dictionary<Type, IExitableState> states;
        private IExitableState activeState;

        public IExitableState ActiveState => activeState;
        public void SetStates(IEnumerable<IExitableState> states)
        {
            this.states = states.ToDictionary(state => state.GetType(), state => state);
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TLoad>(TLoad payLoad) where TState : class, ILoadedState<TLoad>
        {
            TState state = ChangeState<TState>();
            state.Enter(payLoad);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            activeState?.Exit();
            TState state = GetState<TState>();
            activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => states[typeof(TState)] as TState;
    }
}
