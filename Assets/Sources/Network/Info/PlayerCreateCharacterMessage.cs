
using Mirror;
using UnityEngine;

namespace WR.Network.Info
{
    public struct PlayerCreateCharacterMessage : NetworkMessage
    {
        public int id;
        public int num;
        public Color color;
        public string name;
    }
}
