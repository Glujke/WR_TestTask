using System;
using UnityEngine;

namespace WR.Configs
{
    [Serializable]
    public class GameConfig
    {
        [SerializeField]
        public int MinConnection = 3;
        [SerializeField]
        public int MaxConnection = 3;
        [SerializeField]
        public int MaxFlags = 10;
    }
}
