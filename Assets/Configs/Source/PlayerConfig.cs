using System;
using UnityEngine;


namespace WR.Configs
{
    [Serializable]
    public class PlayerConfig
    {
        [SerializeField]
        public float Speed;
        [SerializeField]
        public PlayerInfo[] PlayerInfo;
    }
}
