using System;
using UnityEngine;


namespace WR.Configs
{
    [Serializable]
    public struct PlayerInfo
    {
        [SerializeField]
        public int number;
        [SerializeField]
        public string NameColor;
        [SerializeField]
        public Color Color;
    }
}
