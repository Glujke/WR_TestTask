using UnityEngine;

namespace WR.Configs
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "WR/Settings")]
    public class MainConfig : ScriptableObject
    {
        [SerializeField]
        public GameConfig gameConfig;

        [SerializeField]
        public PathConfig pathConfig;

        [SerializeField]
        public FlagConfig flagConfig;

        [SerializeField]
        public PlayerConfig playerConfig;

        [SerializeField]
        public MiniGameConfig miniGameConfig;
    }
}
