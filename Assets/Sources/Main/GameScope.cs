using UnityEngine;
using VContainer;
using VContainer.Unity;
using WR.States.Machine;
using WR.Movement;
using WR.WR_Input;
using WR.Configs;
using WR.Presenters;
using System.Linq;
using WR.UI;
using WR.Game.Interfaces;
using WR.Network.Items;
using WR.Game.Player;
using WR.Game.Services;

public class GameScope : LifetimeScope
{
    private IContainerBuilder builder;
    private const string NOTIFIER_NAME = "Notifier";
    private const string VARIABLE_JOYSTICK = "Variable Joystick";

    protected override void Configure(IContainerBuilder builder)
    {
        this.builder = builder;
        InitialScopeRegister<PlayerConfig>();
        InitialScopeRegister<GameConfig>();
        InitialScopeRegister<MiniGameConfig>();
        var flagConfig = InitialScopeRegister<FlagConfig>();
        var path = InitialScopeRegister<PathConfig>();

        InitialScopeRegister<StateMachine>();


        InitialScopeRegister<ICountFlag>();

        var player = GetComponent<PlayerNetwork>();
        var flags = FindObjectsOfType<FlagNetwork>().Where(p => p.PlayerId == player.Id).ToList();
        foreach (var flag in flags)
        {
            flag.Init(player, flagConfig.MaxTimeToBlock);
        }

        builder.RegisterComponent(player);

        var prefabVariableJoystick = Resources.Load<GameObject>(path.PathToInputCanvas);
        var joystick = Instantiate(prefabVariableJoystick, Vector3.zero, Quaternion.identity).
                                transform.Find(VARIABLE_JOYSTICK).GetComponent<VariableJoystick>();

        var miniGamePrefab = Resources.Load<GameObject>(path.PathToMiniGamePrefab);
        var miniGameView = Instantiate(miniGamePrefab, Vector3.zero, Quaternion.identity).GetComponent<MiniGameView>();

        var capturerPrefab = Resources.Load<GameObject>(path.PathToCaptureTimerPrefab);
        var capturerView = Instantiate(capturerPrefab, Vector3.zero, Quaternion.identity).GetComponent<CaptureView>();

        var blockedPrefab = Resources.Load<GameObject>(path.PathToBlockTimerPrefab);
        var blockedView = Instantiate(blockedPrefab, Vector3.zero, Quaternion.identity).GetComponent<BlockFlagView>();

        var notifierPrefab = Resources.Load<GameObject>(path.PathToChatPrefab);
        var notifier = Instantiate(notifierPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerNotifier>();
        notifier.name = NOTIFIER_NAME;

        builder.RegisterComponent(notifier);
        builder.RegisterComponent(joystick);
        builder.RegisterComponent(miniGameView);
        builder.RegisterComponent(capturerView);
        builder.RegisterComponent(blockedView);
        builder.Register<IInput, JoystickInput>(Lifetime.Scoped);
        builder.Register<MiniGameRollService>(Lifetime.Scoped);
        builder.Register<IMovement, PlayerMovement>(Lifetime.Scoped);
        builder.Register<IMiniGame, MiniGameService>(Lifetime.Scoped);
        builder.Register<CaptureService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

        builder.RegisterEntryPoint<MiniGamePresenter>();
        builder.RegisterEntryPoint<CapturePresenter>();

    }

    private T InitialScopeRegister<T>()
    {
        var initContainer = Find<InitialScope>().Container;
        var config = initContainer.Resolve<T>();
        builder.RegisterComponent(config);
        return config;
    }
}
