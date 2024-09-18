
using Mirror;
using UnityEngine;

namespace WR.Network.Info
{
    public struct PlayerConnectionInfo : NetworkMessage
    {
        public int id;
        public int num;
        public Color color;
        public string name;
        public bool isReady;

        public static PlayerConnectionInfo Default => new PlayerConnectionInfo() { color = Color.gray, id = -1, isReady = false, name = "" };
    }
}
