using VContainer.Unity;
using VContainer;
using WR.States;
using WR.States.Machine;
using UnityEngine;
using WR.UI;
using System.Collections.Generic;
using WR.Configs;
using WR.Network.Server;
using WR.Game.Flag;
using WR.Game.Interfaces;
using WR.Game.Player;
using WR.Game.Services;

public class InitialScope : LifetimeScope
{
    private class EntryPoint : IStartable
    {
        private readonly IStateMachine stateMachine;
        public EntryPoint(IStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Start()
        {
            stateMachine.Enter<BootState>();
        }
    }
    [SerializeField]
    private MainConfig config;

    [SerializeField]
    private MainMenuView mainMenu;
    [SerializeField]

    private LobbyView lobby;
    [SerializeField]
    private ServerView serverUI;

    [SerializeField]
    private CustomNetworkManager networkManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IFlagPositionGenerating, FlagGenerator>(Lifetime.Scoped);
        builder.Register<ICountFlag, FlagCounterService>(Lifetime.Scoped);
        builder.Register<IPlayerSpawning, PlayerSpawner>(Lifetime.Scoped);
        builder.Register<LobbyConnectionHandler>(Lifetime.Singleton);
        builder.Register<BootState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<ConnectionState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<HostConnectState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<ClientConnectState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<ServerConnectState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<LobbyState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<GameLoadingState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<GameState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<ServerListenState>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        builder.Register<StateMachine>(Lifetime.Singleton).As<IStateMachine>().AsSelf();
        builder.RegisterBuildCallback(container =>
        {
            var stateMachine = container.Resolve<StateMachine>();
            var states = container.Resolve<IEnumerable<IExitableState>>();
            stateMachine.SetStates(states);
        });

        builder.RegisterEntryPoint<EntryPoint>();
        builder.RegisterComponent(mainMenu);
        builder.RegisterComponent(networkManager);
        builder.RegisterComponent(lobby);
        builder.RegisterComponent(serverUI);

        builder.RegisterComponent(config.playerConfig);
        builder.RegisterComponent(config.flagConfig);
        builder.RegisterComponent(config.gameConfig);
        builder.RegisterComponent(config.miniGameConfig);
        builder.RegisterComponent(config.pathConfig);
    }

}